namespace Battlescript;

public class StringVariable(string? value = null) : Variable
{
    public string? Value { get; set; } = value;
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a string variable");
    }
    
    public override Variable GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a string variable");
    }
}