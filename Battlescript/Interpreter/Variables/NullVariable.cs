namespace Battlescript;

public class NullVariable : Variable
{
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a null variable");
    }
    
    public override Variable? GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a null variable");
    }
}