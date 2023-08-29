using BattleScript.Core;

namespace BattleScript.Instructions;

public class NumberInstruction : Instruction
{
    public NumberInstruction(
        int? value = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Number,
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