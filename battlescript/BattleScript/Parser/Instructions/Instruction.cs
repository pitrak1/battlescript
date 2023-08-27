using BattleScript.Core;

namespace BattleScript.Instructions;

public class Instruction
{
    public Consts.InstructionTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public Instruction? Left { get; set; }
    public Instruction? Right { get; set; }
    public Instruction? Next { get; set; }
    public List<Instruction> Instructions { get; set; }
    public int? Line { get; set; }
    public int? Column { get; set; }

    public Instruction(
        Consts.InstructionTypes? type = null,
        dynamic? value = null,
        Instruction? left = null,
        Instruction? right = null,
        Instruction? next = null,
        List<Instruction>? instructions = null
    )
    {
        Type = type;
        Value = value;
        Left = left;
        Right = right;
        Next = next;
        if (instructions is not null)
        {
            Instructions = instructions;
        }
        else
        {
            Instructions = new List<Instruction>();
        }
    }

    public Instruction SetDebugInfo(int? line, int? column)
    {
        Line = line;
        Column = column;
        return this;
    }
}

