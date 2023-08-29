using BattleScript.Core;

namespace BattleScript.Instructions;

public class OperationInstruction : Instruction
{
    public OperationInstruction(
        string? value = null,
        Instruction? left = null,
        Instruction? right = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Operation,
        value,
        left,
        right,
        null,
        null,
        line,
        column
    )
    { }
}