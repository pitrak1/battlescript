namespace Battlescript;

public class IfInstruction : Instruction, IEquatable<IfInstruction>
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
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
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
    
    #region Equality

    public override bool Equals(object? obj) => obj is IfInstruction inst && Equals(inst);

    public bool Equals(IfInstruction? other) =>
        other is not null &&
        Instructions.SequenceEqual(other.Instructions) &&
        Equals(Condition, other.Condition) &&
        Equals(Next, other.Next);

    public override int GetHashCode() => HashCode.Combine(Instructions, Condition, Next);

    public static bool operator ==(IfInstruction? left, IfInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(IfInstruction? left, IfInstruction? right) => !(left == right);

    #endregion
}