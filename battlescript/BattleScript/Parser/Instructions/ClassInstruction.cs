using BattleScript.Core;

namespace BattleScript.Instructions;

public class ClassInstruction : Instruction
{
    public ClassInstruction(
        Instruction? value = null,
        List<Instruction>? instructions = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Class,
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