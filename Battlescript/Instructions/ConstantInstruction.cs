namespace Battlescript;

public class ConstantInstruction : Instruction
{
    public string Value { get; set; }

    public ConstantInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[0].Value;
    }

    public ConstantInstruction(string value) : base([])
    {
        Value = value;
    }

    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (Value == "True" || Value == "False")
        {
            return memory.CreateBsType(Memory.BsTypes.Bool, Value == "True");
        }
        else
        {
            return new ConstantVariable();
        }
    }
}