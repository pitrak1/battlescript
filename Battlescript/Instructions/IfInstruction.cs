namespace Battlescript;

public class IfInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public IfInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new ParserMissingExpectedTokenException(tokens[^1], ":");
        }

        Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
    }

    public IfInstruction(Instruction condition, Instruction? next = null, List<Instruction>? instructions = null) : base([])
    {
        Condition = condition;
        Next = next;
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var condition = Condition.Interpret(memory);
        if (Truthiness.IsTruthy(memory, condition))
        {
            try
            {
                memory.AddScope();
                foreach (var inst in Instructions)
                {
                    inst.Interpret(memory);
                }

                memory.RemoveScope();
            }
            catch (InternalReturnException e)
            {
                memory.RemoveScope();
                throw;
            }
        }
        else if (Next is not null)
        {
            Next.Interpret(memory, instructionContext, objectContext, lexicalContext);
        }

        return null;
    }
}