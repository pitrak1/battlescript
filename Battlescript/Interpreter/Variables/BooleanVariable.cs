namespace Battlescript;

public class BooleanVariable(bool? value = null) : Variable
{
    public bool Value { get; set; } = value ?? false;
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a boolean variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a boolean variable");
    }
}