using BattleScript.Core;

namespace BattleScript.Instructions;

public class FunctionInstruction : Instruction
{
    public FunctionInstruction(
        List<Instruction>? value = null,
        List<Instruction>? instructions = null,
        int? line = null,
        int? column = null
    ) : base(
        Consts.InstructionTypes.Function,
        value,
        null,
        null,
        null,
        instructions,
        line,
        column
    )
    {
        if (Value is null)
        {
            Value = new List<Instruction>();
        }

        if (Instructions is null)
        {
            Instructions = new List<Instruction>();
        }
    }
}