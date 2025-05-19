namespace Battlescript;

public class KeyValuePairVariable(Variable? left = null, Variable? right = null) : Variable
{
    public Variable? Left { get; set; } = left;
    public Variable? Right { get; set; } = right;
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a kvp variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a kvp variable");
    }
}