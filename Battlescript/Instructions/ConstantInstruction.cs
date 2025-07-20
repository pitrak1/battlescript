namespace Battlescript;

public class ConstantInstruction : Instruction
{
    public string Value { get; set; }

    public ConstantInstruction(List<Token> tokens)
    {
        Value = tokens[0].Value;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ConstantInstruction(string value)
    {
        Value = value;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (Value == "True" || Value == "False")
        {
            return BsTypes.Create(memory, "bool", Value == "True");
        }
        else
        {
            return new ConstantVariable();
        }
    }
}