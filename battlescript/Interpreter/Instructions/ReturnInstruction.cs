namespace BattleScript; 

public class ReturnInstruction : Instruction {
    public ReturnInstruction(
        dynamic? value = null
    ) : base(
        Consts.InstructionTypes.Return
    ) {
        Value = value;
    }
}