using BattleScript.Core;

namespace BattleScript.Instructions;

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