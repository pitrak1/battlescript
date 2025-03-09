namespace Battlescript;

public class Instruction(
    Consts.InstructionTypes? type,
    dynamic? literalValue = null,
    string? name = null,
    string? operation = null,
    Instruction? value = null,
    List<Instruction>? valueList = null,
    Instruction? next = null,
    Instruction? left = null,
    Instruction? right = null,
    List<Instruction>? instructions = null,
    int? line = 0,
    int? column = 0)
{
    public int? Line { get; set; } = line;
    public int? Column { get; set; } = column;
    public Consts.InstructionTypes? Type { get; set; } = type;
    public dynamic? LiteralValue { get; set; } = literalValue;
    public string? Name { get; set; } = name;
    public string? Operation { get; set; } = operation;
    public Instruction? Value { get; set; } = value;
    public List<Instruction> ValueList { get; set; } = valueList ?? [];
    public Instruction? Next { get; set; } = next;
    public Instruction? Left { get; set; } = left;
    public Instruction? Right { get; set; } = right;
    public List<Instruction> Instructions { get; set; } = instructions ?? [];
}