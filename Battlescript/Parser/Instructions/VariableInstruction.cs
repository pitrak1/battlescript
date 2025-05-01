namespace Battlescript;

public class VariableInstruction : Instruction 
{
    public string Name { get; set; } 
    public Instruction? Next { get; set; }

    public VariableInstruction(List<Token> tokens)
    {
        Name = tokens[0].Value;
        Next = CheckAndRunFollowingTokens(tokens, 1);
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public VariableInstruction(string name, Instruction? next = null)
    {
        Name = name;
        Next = next;
    }

    public override Variable Interpret(Memory memory, Variable? context = null)
    {
        var variable = memory.GetAndCreateIfNotExists(Name);
        return Next is not null ? Next.Interpret(memory, variable) : variable;
    }
}