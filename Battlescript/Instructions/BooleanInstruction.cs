namespace Battlescript;

public class BooleanInstruction : Instruction
{
    public bool Value { get; set; }

    public BooleanInstruction(List<Token> tokens)
    {
        Value = tokens[0].Value == "True";
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public BooleanInstruction(bool value)
    {
        Value = value;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        return new BooleanVariable(Value);
    }
}