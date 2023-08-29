using BattleScript.Core;

namespace BattleScript.Instructions;

public class DeclarationInstruction : Instruction
{
    public DeclarationInstruction(
        string? value,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Declaration,
        value,
        null,
        null,
        null,
        null,
        line,
        column
    )
    { }
}