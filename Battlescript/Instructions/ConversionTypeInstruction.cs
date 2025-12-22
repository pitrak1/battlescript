namespace Battlescript;

public class ConversionTypeInstruction : Instruction
{
    public string Value { get; set; }
    public List<Instruction> Parameters { get; set; }

    public ConversionTypeInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            var endOfArgumentsIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
            var argumentTokens = tokens.GetRange(2, endOfArgumentsIndex - 2);
            Parameters = InstructionUtilities.ParseEntriesBetweenDelimiters(argumentTokens, [","])!;
        
            if (tokens.Count > endOfArgumentsIndex + 1)
            {
                throw new ParserUnexpectedTokenException(tokens[endOfArgumentsIndex + 1]);
            }
        }
        
        Value = tokens[0].Value;
    }

    public ConversionTypeInstruction(string value, int? line = null, string? expression = null) : base(line, expression)
    {
        Value = value;
    }
    
    public override Variable? Interpret(        
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Value)
        {
            case "__btl_numeric__":
                if (Parameters.Count == 0)
                {
                    return new NumericVariable(0);
                }
                
                if (Parameters.Count > 2)
                {
                    throw new Exception("Too many parameters for __btl_numeric__");
                }

                // This is kind of a hacky workaround.  I'll have to give this one some thought
                bool shouldTruncate = Parameters.Count == 2;

                dynamic value = 0;
                if (Parameters[0] is NumericInstruction numInstruction)
                {
                    value = numInstruction.Value;
                }
                else if (Parameters[0] is StringInstruction stringInstruction)
                {
                    value = stringInstruction.Value.Contains(".") ? float.Parse(stringInstruction.Value) : int.Parse(stringInstruction.Value);
                }
                else if (Parameters[0] is VariableInstruction variableInstruction)
                {
                    var variableValue = closure.GetVariable(callStack, variableInstruction);
                    if (variableValue is StringVariable stringVariable)
                    {
                        value = stringVariable.Value.Contains(".") ? float.Parse(stringVariable.Value) : int.Parse(stringVariable.Value);
                    }
                    else if (variableValue is NumericVariable numericVariable)
                    {
                        value = numericVariable.Value;
                    }
                    else if (BsTypes.Is(BsTypes.Types.String, variableValue))
                    {
                        var stringValue = BsTypes.GetStringValue(variableValue);
                        value = stringValue.Contains(".") ? float.Parse(stringValue) : int.Parse(stringValue);
                    }
                    else if (BsTypes.Is(BsTypes.Types.Int, variableValue))
                    {
                        value = BsTypes.GetIntValue(variableValue);
                    }
                    else if (BsTypes.Is(BsTypes.Types.Float, variableValue))
                    {
                        value = BsTypes.GetFloatValue(variableValue);
                    }
                }

                if (shouldTruncate && value is double)
                {
                    value = (int)Math.Floor(value);
                }
                
                return new NumericVariable(value);
            case "__btl_sequence__":
                return new SequenceVariable();
            case "__btl_mapping__":
                return new MappingVariable();
            case "__btl_string__":
                if (Parameters.Count == 1)
                {
                    var parameter = Parameters[0].Interpret(callStack, closure, instructionContext, objectContext, lexicalContext);
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
        return null;
    }
}