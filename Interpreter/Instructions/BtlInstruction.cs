namespace BattleScript; 

public class BtlInstruction : Instruction {
    public BtlInstruction(
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Btl,
        null,
        null,
        null,
        next
    ) {}
}