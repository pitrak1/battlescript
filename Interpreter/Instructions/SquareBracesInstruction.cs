namespace BattleScript; 

public class SquareBracesInstruction : Instruction {
    public SquareBracesInstruction(
        dynamic? value = null,
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.SquareBraces,
        null,
        null,
        null,
        next
    ) {
        Value = value;
    }
}