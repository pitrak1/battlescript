using BattleScript.Core;

namespace BattleScript.Instructions;

public class NumberInstruction : Instruction
{
    public NumberInstruction(
        dynamic? value = null
    ) : base(
        Consts.InstructionTypes.Number
    )
    {
        Value = value;
    }
}