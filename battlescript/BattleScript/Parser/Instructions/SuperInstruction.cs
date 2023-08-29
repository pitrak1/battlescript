using BattleScript.Core;

namespace BattleScript.Instructions;

public class SuperInstruction : Instruction
{
    public SuperInstruction(
        Instruction? next = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Super,
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