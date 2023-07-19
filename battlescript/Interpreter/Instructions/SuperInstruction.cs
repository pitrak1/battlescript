namespace BattleScript; 

public class SuperInstruction : Instruction {
    public SuperInstruction(
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Super,
        null,
        null,
        null,
        next
    ) {}
}