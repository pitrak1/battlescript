namespace BattleScript.Core;
public class DictionaryInstruction : Instruction
{
    public DictionaryInstruction(
        dynamic? value = null,
        Instruction? next = null
    ) : base(
        Consts.InstructionTypes.Dictionary,
        null,
        null,
        null,
        next
    )
    {
        Value = value;
    }
}