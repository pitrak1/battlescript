using BattleScript.Core;

namespace BattleScript.Instructions;

public class StringInstruction : Instruction
{
    public StringInstruction(
        string? value = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.String,
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