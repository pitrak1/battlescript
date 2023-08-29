using BattleScript.Core;

namespace BattleScript.Instructions;

public class BooleanInstruction : Instruction
{
    public BooleanInstruction(
        bool? value = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Boolean,
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