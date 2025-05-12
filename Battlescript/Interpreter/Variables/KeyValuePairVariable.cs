namespace Battlescript;

public class KeyValuePairVariable(Variable? left = null, Variable? right = null) : Variable
{
    public Variable? Left { get; set; } = left;
    public Variable? Right { get; set; } = right;
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a kvp variable");
    }
    
    public override Variable? GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a kvp variable");
    }
}