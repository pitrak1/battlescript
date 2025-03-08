namespace Battlescript;

public class Instruction
{
    public int? Line { get; set; }
    public int? Column { get; set; }
    public Consts.InstructionTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public Instruction? Next { get; set; }
    public Instruction? Left { get; set; }
    public Instruction? Right { get; set; }
    public List<Instruction> Instructions { get; set; }
    public List<Instruction> Values { get; set; }

    public Instruction(
        int? line,
        int? column,
        Consts.InstructionTypes? type,
        dynamic? value,
        Instruction? next = null,
        Instruction? left = null,
        Instruction? right = null,
        List<Instruction>? instructions = null,
        List<Instruction>? values = null
    )
    {
        Line = line;
        Column = column;
        Type = type;
        Value = value;
        Next = next;
        Left = left;
        Right = right;
        Instructions = instructions ?? [];
        Values = values ?? [];
    }
    
    // Used for testing so that column and line default to 0
    public Instruction(
        Consts.InstructionTypes? type,
        dynamic? value,
        Instruction? next = null,
        Instruction? left = null,
        Instruction? right = null,
        List<Instruction>? instructions = null,
        List<Instruction>? values = null,
        int? line = 0,
        int? column = 0
    )
    {
        Line = line;
        Column = column;
        Type = type;
        Value = value;
        Next = next;
        Left = left;
        Right = right;
        Instructions = instructions ?? [];
        Values = values ?? [];
    }
}