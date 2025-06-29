namespace Battlescript;

public class CommaSeparatedArrayInstruction : GenericArrayInstruction<Instruction?>
{
    
    public CommaSeparatedArrayInstruction(List<Token> tokens)
    {
        PopulateValues(tokens, ",");
    }

    public CommaSeparatedArrayInstruction(List<Instruction?> values)
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
        return new ArrayVariable(",", values);
    }
}