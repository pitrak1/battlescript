namespace Battlescript;

public class BooleanVariable(bool? value = null) : Variable
{
    public bool Value { get; set; } = value ?? false;
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a boolean variable");
    }
    
    public override Variable? GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a boolean variable");
    }
}