using BattleScript.Core;

namespace BattleScript.Instructions;

public class SelfInstruction : Instruction
{
    public SelfInstruction(
        Instruction? next = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Self,
        null,
        null,
        null,
        next,
        null,
        line,
        column
    )
    { }
}