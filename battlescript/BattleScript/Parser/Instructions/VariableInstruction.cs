using BattleScript.Core;

namespace BattleScript.Instructions;

public class VariableInstruction : Instruction
{
    public VariableInstruction(
        string? value = null,
        Instruction? next = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Variable,
        value,
        null,
        null,
        next,
        null,
        line,
        column
    )
    { }
}