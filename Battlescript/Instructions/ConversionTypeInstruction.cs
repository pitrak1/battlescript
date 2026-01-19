namespace Battlescript;

public class ConversionTypeInstruction : Instruction
{
    public string Value { get; set; }
    public List<Instruction> Parameters { get; set; } = [];

    public ConversionTypeInstruction(List<Token> tokens) : base(tokens)
    {
        ParseArguments(tokens);
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

    public ConversionTypeInstruction(
        string value, 
        List<Instruction>? parameters = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Value = value;
        Parameters = parameters ?? [];
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
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
            else if (BsTypes.Is(BsTypes.Types.String, parameter))
            {
                return new StringVariable(BsTypes.GetStringValue(parameter));
            }
            else if (BsTypes.Is(BsTypes.Types.Int, parameter))
            {
                return new StringVariable(BsTypes.GetIntValue(parameter).ToString());
            }
            else if (BsTypes.Is(BsTypes.Types.Float, parameter))
            {
                return new StringVariable(BsTypes.GetFloatValue(parameter).ToString());
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
        else if (BsTypes.Is(BsTypes.Types.String, variable!))
        {
            return ConvertStringToIntOrDouble(BsTypes.GetStringValue(variable!));
        }
        else if (BsTypes.Is(BsTypes.Types.Int, variable!))
        {
            return BsTypes.GetIntValue(variable!);
        }
        else if (BsTypes.Is(BsTypes.Types.Float, variable!))
        {
            return BsTypes.GetFloatValue(variable!);
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
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ConversionTypeInstruction);
    public bool Equals(ConversionTypeInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var parametersEqual = Parameters.SequenceEqual(inst.Parameters);
        return parametersEqual && Value == inst.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Value);
}