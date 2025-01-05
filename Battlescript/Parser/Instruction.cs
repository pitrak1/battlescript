namespace Battlescript;

public class Instruction(
    int? line, 
    int? column, 
    Consts.InstructionTypes? type, 
    dynamic? value, 
    Instruction? left = null, 
    Instruction? right = null,
    List<Instruction>? instructions = null
)
{
    public int? Line { get; set; } = line;
    public int? Column { get; set; } = column;
    public Consts.InstructionTypes? Type { get; set; } = type;
    public dynamic? Value { get; set; } = value;
    public List<Instruction> Instructions { get; set; } = instructions ?? [];
    public Instruction? Left { get; set; } = left;
    public Instruction? Right { get; set; } = right;
}