namespace BattleScript; 

public class ConstructorInstruction : Instruction {
    public ConstructorInstruction(
        dynamic? value = null,
        List<Instruction>? instructions = null
    ) : base(
        Consts.InstructionTypes.Constructor,
        null,
        null,
        null,
        null,
        instructions
    ) {
        Value = value;
    }
}