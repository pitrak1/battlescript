namespace BattleScript; 

public class BooleanInstruction : Instruction {
    public BooleanInstruction(
        dynamic? value = null
    ) : base(
        Consts.InstructionTypes.Boolean
    ) {
        Value = value;
    }
}