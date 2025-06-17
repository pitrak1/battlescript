namespace Battlescript;

public class NoneVariable : ReferenceVariable
{
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a null variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a null variable");
    }
}