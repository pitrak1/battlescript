namespace Battlescript;

public class PrincipleTypeInstruction : Instruction
{
    public string Value { get; set; }
    public List<Instruction> Parameters { get; set; }

    public PrincipleTypeInstruction(List<Token> tokens) : base(tokens)
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

    public PrincipleTypeInstruction(string value) : base([])
    {
        Value = value;
    }
    
    public override Variable? Interpret(        
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Value)
        {
            case "__numeric__":
                if (Parameters.Count == 0)
                {
                    return new NumericVariable(0);
                }
                
                if (Parameters.Count > 2)
                {
                    throw new Exception("Too many parameters for __numeric__");
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
                    var variableValue = memory.GetVariable(variableInstruction);
                    if (variableValue is StringVariable stringVariable)
                    {
                        value = stringVariable.Value.Contains(".") ? float.Parse(stringVariable.Value) : int.Parse(stringVariable.Value);
                    }
                    else if (variableValue is NumericVariable numericVariable)
                    {
                        value = numericVariable.Value;
                    }
                    else if (memory.Is(Memory.BsTypes.String, variableValue))
                    {
                        var stringValue = memory.GetStringValue(variableValue);
                        value = stringValue.Contains(".") ? float.Parse(stringValue) : int.Parse(stringValue);
                    }
                    else if (memory.Is(Memory.BsTypes.Int, variableValue))
                    {
                        value = memory.GetIntValue(variableValue);
                    }
                    else if (memory.Is(Memory.BsTypes.Float, variableValue))
                    {
                        value = memory.GetFloatValue(variableValue);
                    }
                }

                if (shouldTruncate && value is double)
                {
                    value = (int)Math.Floor(value);
                }
                
                return new NumericVariable(value);
            case "__sequence__":
                return new SequenceVariable();
            case "__mapping__":
                return new MappingVariable();
            case "__string__":
                if (Parameters.Count == 1)
                {
                    var parameter = Parameters[0].Interpret(memory, instructionContext, objectContext, lexicalContext);
                    if (parameter is StringVariable stringVariable)
                    {
                        return stringVariable;
                    }
                    else if (parameter is NumericVariable numericVariable)
                    {
                        return new StringVariable(numericVariable.Value.ToString());
                    }
                    else if (memory.Is(Memory.BsTypes.String, parameter))
                    {
                        return new StringVariable(memory.GetStringValue(parameter));
                    }
                    else if (memory.Is(Memory.BsTypes.Int, parameter))
                    {
                        return new StringVariable(memory.GetIntValue(parameter).ToString());
                    }
                    else if (memory.Is(Memory.BsTypes.Float, parameter))
                    {
                        return new StringVariable(memory.GetFloatValue(parameter).ToString());
                    }
                }
                return new StringVariable();
        }
        return null;
    }
}