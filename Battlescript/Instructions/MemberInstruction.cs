namespace Battlescript;

public class MemberInstruction : Instruction, IEquatable<MemberInstruction>
{
    public string Value { get; set; }

    public MemberInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[1].Value;
        ParseNext(tokens, 2);
    }

    public MemberInstruction(
        string value,
        Instruction? next = null,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Value = value;
        Next = next;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        return instructionContext.GetMember(callStack, closure, this, instructionContext as ObjectVariable);
    }

    #region Equality

    public override bool Equals(object? obj) => obj is MemberInstruction inst && Equals(inst);

    public bool Equals(MemberInstruction? other) =>
        other is not null && Value == other.Value && Equals(Next, other.Next);

    public override int GetHashCode() => HashCode.Combine(Value, Next);

    public static bool operator ==(MemberInstruction? left, MemberInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(MemberInstruction? left, MemberInstruction? right) => !(left == right);

    #endregion
}