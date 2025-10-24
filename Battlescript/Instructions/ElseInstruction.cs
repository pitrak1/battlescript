namespace Battlescript;

public class ElseInstruction : Instruction
{
    public Instruction? Condition { get; set; }

    public ElseInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "invalid syntax");
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
                foreach (var inst in Instructions)
                {
                    inst.Interpret(memory);
                }

            } else if (Next is not null)
            {
                return Next.Interpret(memory, instructionContext, objectContext, lexicalContext);
            }
        }
        else
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(memory);
            }
        }
        
        return new ConstantVariable();
    }
}