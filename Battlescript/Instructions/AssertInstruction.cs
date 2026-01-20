namespace Battlescript;

public class AssertInstruction : Instruction, IEquatable<AssertInstruction>
{
    public Instruction Condition { get; set; }

    public AssertInstruction(List<Token> tokens) : base(tokens)
    {
        var tokensAfterAssertKeyword = tokens.GetRange(1, tokens.Count - 1);
        Condition = InstructionFactory.Create(tokensAfterAssertKeyword)!;
    }

    public AssertInstruction(
        Instruction condition,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Condition = condition;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var condition = Condition.Interpret(callStack, closure);

        if (!Truthiness.IsTruthy(callStack, closure, condition!, this))
        {
            throw new InternalRaiseException(BtlTypes.Types.AssertionError, "");
        }

        return null;
    }

    #region Equality

    public override bool Equals(object? obj) => obj is AssertInstruction inst && Equals(inst);

    public bool Equals(AssertInstruction? other) =>
        other is not null && Equals(Condition, other.Condition);

    public override int GetHashCode() => Condition.GetHashCode();

    public static bool operator ==(AssertInstruction? left, AssertInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(AssertInstruction? left, AssertInstruction? right) => !(left == right);

    #endregion
}