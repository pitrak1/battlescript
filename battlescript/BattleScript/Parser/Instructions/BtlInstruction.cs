using BattleScript.Core;

namespace BattleScript.Instructions;

public class BtlInstruction : Instruction
{
    public BtlInstruction(
        Instruction? next = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Btl,
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