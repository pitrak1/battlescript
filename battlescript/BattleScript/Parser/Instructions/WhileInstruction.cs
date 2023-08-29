using BattleScript.Core;

namespace BattleScript.Instructions;

public class WhileInstruction : Instruction
{
    public WhileInstruction(
        Instruction? value = null,
        List<Instruction>? instructions = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.While,
        value,
        null,
        null,
        null,
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