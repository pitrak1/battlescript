using BattleScript.Core;

namespace BattleScript.Instructions;

public class AssignmentInstruction : Instruction
{
    public AssignmentInstruction(
        Instruction? left = null,
        Instruction? right = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Assignment,
        null,
        left,
        right,
        null,
        null,
        line,
        column
    )
    { }
}

