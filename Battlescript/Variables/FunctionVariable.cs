namespace Battlescript;

public class FunctionVariable(List<Instruction>? parameters, List<Instruction>? instructions) : Variable
{
    public List<Instruction> Parameters { get; set; } = parameters ?? [];
    public List<Instruction> Instructions { get; set; } = instructions ?? [];
    
    public override void AssignToIndexOrKey(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a function variable");
    }
    
    public override Variable? GetIndex(Memory memory, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a function variable");
    }
}