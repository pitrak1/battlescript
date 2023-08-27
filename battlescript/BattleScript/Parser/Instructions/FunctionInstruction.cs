using BattleScript.Core;

namespace BattleScript.Instructions;

public class FunctionInstruction : Instruction
{
    public FunctionInstruction(
        dynamic? value = null,
        List<Instruction>? instructions = null
    ) : base(
        Consts.InstructionTypes.Function,
        null,
        null,
        null,
        null,
        instructions
    )
    {
        Value = value;
    }
}