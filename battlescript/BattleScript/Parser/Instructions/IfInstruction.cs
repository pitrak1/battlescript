using BattleScript.Core;

namespace BattleScript.Instructions;

public class IfInstruction : Instruction
{
    public IfInstruction(
        Instruction? value = null,
        List<Instruction>? instructions = null,
        Instruction? next = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.If,
        value,
        null,
        null,
        next,
        instructions,
        line,
        column
    )
    {
        if (Instructions is null)
        {
            Instructions = new List<Instruction>();
        }
    }
}