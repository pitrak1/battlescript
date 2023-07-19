namespace BattleScript; 

public class StringInstruction : Instruction {
    public StringInstruction(
        dynamic? value = null
    ) : base(
        Consts.InstructionTypes.String
    ) {
        Value = value;
    }
}