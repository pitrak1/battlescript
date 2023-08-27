using BattleScript.Core;

namespace BattleScript.Instructions;

public class IfInstruction : Instruction
{
    public IfInstruction(
        dynamic? value = null,
        List<Instruction>? instructions = null,
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.If,
        null,
        null,
        null,
        next,
        instructions
    )
    {
        Value = value;
    }
}