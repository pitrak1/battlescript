namespace Battlescript;

public class NumberInstruction : Instruction
{
    public double Value { get; set; }

    public NumberInstruction(List<Token> tokens)
    {
        CheckForNoFollowingTokens(tokens, 1);
        
        Value = Convert.ToDouble(tokens[0].Value);
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public NumberInstruction(double value)
    {
        Value = value;
    }
    
    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        return new Variable(Consts.VariableTypes.Number, Value);
    }
}