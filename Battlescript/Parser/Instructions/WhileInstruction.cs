namespace Battlescript;

public class WhileInstruction : Instruction 
{
    public Instruction Condition { get; set; }

    public WhileInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("While statement should end with colon", tokens[0]);
        }
    
        Condition = Parse(tokens.GetRange(1, tokens.Count - 2));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public WhileInstruction(Instruction condition)
    {
        Condition = condition;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var condition = Condition.Interpret(memory);
        while (InterpreterUtilities.IsVariableTruthy(condition))
        {
            memory.AddScope();
            foreach (var inst in Instructions)
            {
                inst.Interpret(memory);
            }
            memory.RemoveScope();
            condition = Condition.Interpret(memory);
        }

        return new NullVariable();
    }
}