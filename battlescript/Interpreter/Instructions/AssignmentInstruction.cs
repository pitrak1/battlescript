namespace BattleScript; 

public class AssignmentInstruction : Instruction {
    public AssignmentInstruction(
        Instruction? left = null,
        Instruction? right = null
    ) : base(
        Consts.InstructionTypes.Assignment,
        null,
        left,
        right
    ) {}
}

