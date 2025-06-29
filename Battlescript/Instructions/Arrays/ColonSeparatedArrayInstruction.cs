namespace Battlescript;

public class ColonSeparatedArrayInstruction : GenericArrayInstruction<Instruction?>
{
    public ColonSeparatedArrayInstruction(List<Token> tokens)
    {
        PopulateValues(tokens, ":");
    }

    public ColonSeparatedArrayInstruction(List<Instruction?> values)
    {
        PopulateValues(values);
    }
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var values = new List<Variable?>();
        foreach (var instruction in Instructions)
        {
            values.Add(instruction.Interpret(memory, instructionContext, objectContext, lexicalContext));
        }
        return new ArrayVariable(":", values);
    }
}