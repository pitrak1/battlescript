namespace BattleScript; 

public class SelfInstruction : Instruction {
    public SelfInstruction(
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Self,
        null,
        null,
        null,
        next
    ) {}
}