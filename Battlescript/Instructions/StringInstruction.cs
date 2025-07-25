namespace Battlescript;

public class StringInstruction : Instruction
{
    public string Value { get; set; }

    public StringInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[0].Value;
    }

    public StringInstruction(string value) : base([])
    {
        Value = value;
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return memory.CreateBsType(Memory.BsTypes.String, Value);
    }
}