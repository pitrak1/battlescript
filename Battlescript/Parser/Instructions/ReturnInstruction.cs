namespace Battlescript;

public class ReturnInstruction : Instruction 
{
    public Instruction? Value { get; set; }

    public ReturnInstruction(List<Token> tokens)
    {
        Value = Parse(tokens.GetRange(1, tokens.Count - 1));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ReturnInstruction(Instruction? value)
    {
        Value = value;
    }

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        var returnVariable = memory.GetAndCreateIfNotExists("return");
        var returnValue = Value.Interpret(memory);
        return returnVariable.Set(returnValue);
    }
}