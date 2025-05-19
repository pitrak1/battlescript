namespace Battlescript;

public class StringVariable(string? value = null) : Variable
{
    public string? Value { get; set; } = value;
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a string variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a string variable");
    }
}