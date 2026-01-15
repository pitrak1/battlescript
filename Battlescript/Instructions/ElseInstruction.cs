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
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ElseInstruction);
    public bool Equals(ElseInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return instructionsEqual && Condition == inst.Condition && Equals(Next, inst.Next);
    }
    
    public override int GetHashCode()
    {
        int hash = 55;

        for (int i = 0; i < Instructions.Count; i++)
        {
            hash += Instructions[i].GetHashCode() * 85 * (i + 1);
        }
        
        hash += Condition?.GetHashCode() * 98 ?? 33;
        hash += Next?.GetHashCode() * 10 ?? 7;
        return hash;
    }
}