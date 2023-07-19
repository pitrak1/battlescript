namespace BattleScript; 

public class DeclarationInstruction : Instruction {
    public DeclarationInstruction(
        dynamic? value
    ) : base(
        Consts.InstructionTypes.Declaration
    ) {
        Value = value;
    }
}