using BattleScript.Core;

namespace BattleScript.Instructions;

public class SquareBracesInstruction : Instruction
{
    public SquareBracesInstruction(
        List<Instruction>? value = null,
        Instruction? next = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.SquareBraces,
        value,
        null,
        null,
        next,
        null,
        line,
        column
    )
    {
        if (Value is null)
        {
            Value = new List<Instruction>();
        }
    }
}