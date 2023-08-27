using BattleScript.Core;

namespace BattleScript.Instructions;

public class StringInstruction : Instruction
{
    public StringInstruction(
        dynamic? value = null
    ) : base(
        Consts.InstructionTypes.String
    )
    {
        Value = value;
    }
}