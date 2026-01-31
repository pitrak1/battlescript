using Battlescript.BuiltIn;

namespace Battlescript;

public class BindingInstruction : Instruction, IEquatable<BindingInstruction>
{
    public string Value { get; set; }
    public List<Instruction>? Parameters { get; set; }

    public BindingInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            ParseArguments(tokens);
        }

        Value = tokens[0].Value;
    }

    private void ParseArguments(List<Token> tokens)
    {
        if (tokens.Count > 1)
        {
            var tokensAfterTypeName = tokens.GetRange(1, tokens.Count - 1);
            var argumentTokens = InstructionUtilities.GetGroupedTokensAtStart(tokensAfterTypeName);
            Parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
            VerifyNoTokensAfterArguments(tokensAfterTypeName, argumentTokens.Count);
        }
    }

    private void VerifyNoTokensAfterArguments(List<Token> tokens, int argumentTokensCount)
    {
        // argument tokens + the two parentheses + the type name
        if (tokens.Count > argumentTokensCount + 3)
        {
            throw new ParserUnexpectedTokenException(tokens[argumentTokensCount + 3]);
        }
    }

    public BindingInstruction(
        string value, 
        List<Instruction>? parameters = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Value = value;
        Parameters = parameters;
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        if (Parameters is null)
        {
            return new BindingVariable(Value);
        }
        
        switch (Value)
        {
            case "__btl_numeric__":
                return InterpretNumeric(callStack, closure, instructionContext);
            case "__btl_sequence__":
                return new SequenceVariable();
            case "__btl_mapping__":
                return new MappingVariable();
            case "__btl_string__":
                return InterpretString(callStack, closure, instructionContext);
            case "__btl_getattr__":
                return BuiltInGetAttr.Run(callStack, closure, Parameters);
            case "__btl_isinstance__":
                return BuiltInIsInstance.Run(callStack, closure, Parameters);
            case "__btl_issubclass__":
                return BuiltInIsSubclass.Run(callStack, closure, Parameters);
            case "__btl_len__":
                return BuiltInLen.Run(callStack, closure, Parameters);
            case "__btl_print__":
                BuiltInPrint.Run(callStack, closure, Parameters);
                return null;
            case "__btl_type__":
                return BuiltInType.Run(callStack, closure, Parameters);
            case "__btl_callable__":
                return BuiltInCallable.Run(callStack, closure, Parameters);
            case "__btl_delattr__":
                BuiltInDelAttr.Run(callStack, closure, Parameters);
                return null;
            case "__btl_dict_keys__":
                return BuiltInDictKeys.Run(callStack, closure, Parameters);
        }
        return null;
    }

    private Variable InterpretString(CallStack callStack, Closure closure, Variable? instructionContext = null)
    {
        if (Parameters.Count > 0)
        {
            var parameter = Parameters[0].Interpret(callStack, closure, instructionContext);
            if (parameter is StringVariable stringVariable)
            {
                return stringVariable;
            }
            else if (parameter is NumericVariable numericVariable)
            {
                return new StringVariable(numericVariable.Value.ToString());
            }
            else if (BtlTypes.Is(BtlTypes.Types.String, parameter))
            {
                return new StringVariable(BtlTypes.GetStringValue(parameter));
            }
            else if (BtlTypes.Is(BtlTypes.Types.Int, parameter))
            {
                return new StringVariable(BtlTypes.GetIntValue(parameter).ToString());
            }
            else if (BtlTypes.Is(BtlTypes.Types.Float, parameter))
            {
                return new StringVariable(BtlTypes.GetFloatValue(parameter).ToString());
            }
        }
        return new StringVariable();
    }

    private Variable InterpretNumeric(CallStack callStack, Closure closure, Variable? instructionContext = null)
    {
        if (Parameters.Count == 0)
        {
            return new NumericVariable(0);
        }
        
        // The reason we have to check instructions instead of interpreting and checking the variables
        // is that conversion types are used in the built-in types, so interpreting a number will not
        // work if we haven't defined int or float
        dynamic value = GetValueFromInstruction(closure, callStack, Parameters[0]);
        
        // Because we want to be able to truncate to an integer, we pass a second argument of True
        if (Parameters.Count == 2 && value is double)
        {
            value = (int)Math.Floor(value);
        }
        
        return new NumericVariable(value);
    }

    private dynamic GetValueFromInstruction(Closure closure, CallStack callStack, Instruction instruction)
    {
        if (instruction is NumericInstruction numInstruction)
        {
            return numInstruction.Value;
        }
        else if (instruction is StringInstruction stringInstruction)
        {
            return ConvertStringToIntOrDouble(stringInstruction.Value);
        }
        else if (instruction is VariableInstruction variableInstruction)
        {
            var variableValue = closure.GetVariable(callStack, variableInstruction);
            return GetValueFromVariable(variableValue);
        }
        else
        {
            return 0;
        }
    }

    private dynamic GetValueFromVariable(Variable? variable)
    {
        if (variable is StringVariable stringVariable)
        {
            return ConvertStringToIntOrDouble(stringVariable.Value);
        }
        else if (variable is NumericVariable numericVariable)
        {
            return numericVariable.Value;
        }
        else if (BtlTypes.Is(BtlTypes.Types.String, variable!))
        {
            return ConvertStringToIntOrDouble(BtlTypes.GetStringValue(variable!));
        }
        else if (BtlTypes.Is(BtlTypes.Types.Int, variable!))
        {
            return BtlTypes.GetIntValue(variable!);
        }
        else if (BtlTypes.Is(BtlTypes.Types.Float, variable!))
        {
            return BtlTypes.GetFloatValue(variable!);
        }
        else
        {
            return 0;
        }
    }

    private dynamic ConvertStringToIntOrDouble(string value)
    {
        return value.Contains(".") ? double.Parse(value) : int.Parse(value);
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is BindingInstruction inst && Equals(inst);

    public bool Equals(BindingInstruction? other) =>
        other is not null &&
        Value == other.Value &&
        (Parameters is null && other.Parameters is null ||
         Parameters is not null && other.Parameters is not null && Parameters.SequenceEqual(other.Parameters));

    public override int GetHashCode() => HashCode.Combine(Parameters, Value);

    public static bool operator ==(BindingInstruction? left, BindingInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(BindingInstruction? left, BindingInstruction? right) => !(left == right);

    #endregion
}