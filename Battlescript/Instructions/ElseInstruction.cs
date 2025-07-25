namespace Battlescript;

public class ElseInstruction : Instruction
{
    public Instruction? Condition { get; set; }

    public ElseInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new ParserMissingExpectedTokenException(tokens[^1], ":");
        }

        if (tokens[0].Value == "elif")
        {
            Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
        }
    }

    public ElseInstruction(Instruction? condition, Instruction? next, List<Instruction>? instructions = null) : base([])
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
        if (Condition is not null)
        {
            var condition = Condition.Interpret(memory);
            if (Truthiness.IsTruthy(memory, condition))
            {
                CreateScopeAndRunInstructions();

            } else if (Next is not null)
            {
                return Next.Interpret(memory, instructionContext, objectContext, lexicalContext);
            }
        }
        else
        {
            CreateScopeAndRunInstructions();
        }
        
        return new ConstantVariable();

        void CreateScopeAndRunInstructions()
        {
            try {
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
    }
}