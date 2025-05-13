namespace Battlescript;

public class IfInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public IfInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("If statement should end with colon", tokens[0]);
        }

        Condition = Parse(tokens.GetRange(1, tokens.Count - 2));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public IfInstruction(Instruction condition, List<Instruction>? instructions = null)
    {
        Condition = condition;
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var condition = Condition.Interpret(memory);
        if (InterpreterUtilities.IsVariableTruthy(condition))
        {
            memory.AddScope();
            foreach (var inst in Instructions)
            {
                inst.Interpret(memory);
            }
            memory.RemoveScope();
        }

        return new NullVariable();
    }
}