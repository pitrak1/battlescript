using BattleScript.Core;

namespace BattleScript.Instructions;

public class ElseInstruction : Instruction
{
    public ElseInstruction(
        dynamic? value = null,
        List<Instruction>? instructions = null,
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Else
    )
    {
        Value = value;
        if (instructions is not null)
        {
            Instructions = instructions;
        }
        else
        {
            Instructions = new List<Instruction>();
        }
        Next = next;
    }
}