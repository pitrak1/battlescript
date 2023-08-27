using BattleScript.Core;

namespace BattleScript.Instructions;

public class AssignmentInstruction : Instruction
{
    public AssignmentInstruction(
        Instruction? left = null,
        Instruction? right = null
    ) : base(
        Consts.InstructionTypes.Assignment,
        null,
        left,
        right
    )
    { }
}

