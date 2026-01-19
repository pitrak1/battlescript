namespace Battlescript;

public class IfInstruction : Instruction
{
    public Instruction Condition { get; set; }

    public IfInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        Condition = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
    }

    public IfInstruction(
        Instruction condition, 
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
        var condition = Condition.Interpret(callStack, closure);
        if (Truthiness.IsTruthy(callStack, closure, condition, this))
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(callStack, closure);
            }
        }
        else if (Next is not null)
        {
            Next.Interpret(callStack, closure, instructionContext);
        }

        return null;
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as IfInstruction);
    public bool Equals(IfInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return instructionsEqual && Condition.Equals(inst.Condition) && Equals(Next, inst.Next);
    }
    
    public override int GetHashCode() => HashCode.Combine(Instructions, Condition, Next);
}