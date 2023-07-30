namespace BattleScript.Core;
public class DeclarationInstruction : Instruction
{
    public DeclarationInstruction(
        dynamic? value
    ) : base(
        Consts.InstructionTypes.Declaration
    )
    {
        Value = value;
    }
}