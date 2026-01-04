namespace Battlescript;

public class ElseInstruction : Instruction
{
    public Instruction? Condition { get; set; }

    public ElseInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[0].Value == "elif")
        {
            Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
        }
    }

    public ElseInstruction(
        Instruction? condition = null, 
        Instruction? next = null, 
        List<Instruction>? instructions = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Condition = condition;
        Next = next;
        Instructions = instructions ?? [];
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        if (Condition is not null)
        {
            var condition = Condition.Interpret(callStack, closure);
            if (Truthiness.IsTruthy(callStack, closure, condition, this))
            {
                foreach (var inst in Instructions)
                {
                    inst.Interpret(callStack, closure);
                }

            } else if (Next is not null)
            {
                return Next.Interpret(callStack, closure, instructionContext);
            }
        }
        else
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(callStack, closure);
            }
        }
        
        return new ConstantVariable();
    }
}