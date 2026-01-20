namespace Battlescript;

public class BreakInstruction() : Instruction([]), IEquatable<BreakInstruction>
{
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        throw new InternalBreakException();
    }

    #region Equality

    public override bool Equals(object? obj) => obj is BreakInstruction inst && Equals(inst);

    public bool Equals(BreakInstruction? other) => other is not null;

    public override int GetHashCode() => 87;

    public static bool operator ==(BreakInstruction? left, BreakInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(BreakInstruction? left, BreakInstruction? right) => !(left == right);

    #endregion
}