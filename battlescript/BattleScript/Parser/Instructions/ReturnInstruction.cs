using BattleScript.Core;

namespace BattleScript.Instructions;

public class ReturnInstruction : Instruction
{
    public ReturnInstruction(
        dynamic? value = null
    ) : base(
        Consts.InstructionTypes.Return
    )
    {
        Value = value;
    }
}