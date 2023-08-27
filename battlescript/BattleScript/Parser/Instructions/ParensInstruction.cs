namespace BattleScript.Core;
public class ParensInstruction : Instruction
{
    public ParensInstruction(
        dynamic? value = null,
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Parens,
        null,
        null,
        null,
        next
    )
    {
        Value = value;
    }
}