using BattleScript.Core;

namespace BattleScript.Instructions;

public class ReturnInstruction : Instruction
{
    public ReturnInstruction(
        Instruction? value = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Return,
        value,
        null,
        null,
        null,
        null,
        line,
        column
    )
    { }
}