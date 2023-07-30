namespace BattleScript.Core;
public class WhileInstruction : Instruction
{
    public WhileInstruction(
        dynamic? value = null,
        List<Instruction>? instructions = null
    ) : base(
        Consts.InstructionTypes.While,
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