using BattleScript.Core;

namespace BattleScript.Instructions;

public class VariableInstruction : Instruction
{
    public VariableInstruction(
        dynamic? value = null,
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Variable,
        null,
        null,
        null,
        next
    )
    {
        Value = value;
    }
}