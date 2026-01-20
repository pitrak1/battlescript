namespace Battlescript;

public class ReturnInstruction : Instruction, IEquatable<ReturnInstruction>
{
    public Instruction? Value { get; set; }

    public ReturnInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 1));
        }
    }

    public ReturnInstruction(
        Instruction? value,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Value = value;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var returnValue = Value?.Interpret(callStack, closure);
        throw new InternalReturnException(returnValue);
    }

    #region Equality

    public override bool Equals(object? obj) => obj is ReturnInstruction inst && Equals(inst);

    public bool Equals(ReturnInstruction? other) =>
        other is not null && Equals(Value, other.Value);

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public static bool operator ==(ReturnInstruction? left, ReturnInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ReturnInstruction? left, ReturnInstruction? right) => !(left == right);

    #endregion
}