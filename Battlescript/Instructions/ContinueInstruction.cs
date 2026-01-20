namespace Battlescript;

public class ContinueInstruction() : Instruction([]), IEquatable<ContinueInstruction>
{
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        throw new InternalContinueException();
    }

    #region Equality

    public override bool Equals(object? obj) => obj is ContinueInstruction inst && Equals(inst);

    public bool Equals(ContinueInstruction? other) => other is not null;

    public override int GetHashCode() => 66;

    public static bool operator ==(ContinueInstruction? left, ContinueInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ContinueInstruction? left, ContinueInstruction? right) => !(left == right);

    #endregion
}