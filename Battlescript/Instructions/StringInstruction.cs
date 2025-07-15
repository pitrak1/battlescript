namespace Battlescript;

public class StringInstruction : Instruction
{
    public string Value { get; set; }

    public StringInstruction(List<Token> tokens)
    {
        Value = tokens[0].Value;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public StringInstruction(string value)
    {
        Value = value;
    }
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new StringVariable(Value);
    }
}