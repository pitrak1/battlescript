using BattleScript.Core;

namespace BattleScript.Instructions;

public class OperationInstruction : Instruction
{
    public OperationInstruction(
        dynamic? value = null,
        Instruction? left = null,
        Instruction? right = null
    ) : base(
        Consts.InstructionTypes.Operation,
        null,
        left,
        right
    )
    {
        Value = value;
    }
}