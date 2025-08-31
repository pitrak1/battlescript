namespace Battlescript;

public class StringInstruction : Instruction
{
    public string Value { get; set; }
    public bool IsFormatted { get; set; }

    public StringInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[0].Value;
        IsFormatted = tokens[0].Type == Consts.TokenTypes.FormattedString;
    }

    public StringInstruction(string value, bool isFormatted = false) : base([])
    {
        Value = value;
        IsFormatted = isFormatted;
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return memory.Create(Memory.BsTypes.String, Value);
    }
}