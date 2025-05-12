namespace Battlescript;

public class NumberVariable(double value) : Variable
{
    public double Value { get; set; } = value;
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a number variable");
    }
    
    public override Variable? GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a number variable");
    }
}