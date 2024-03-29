namespace BattleScript.Core;
public class ClassInstruction : Instruction
{
    public ClassInstruction(
        dynamic? value = null,
        List<Instruction>? instructions = null
    ) : base(
        Consts.InstructionTypes.Class,
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