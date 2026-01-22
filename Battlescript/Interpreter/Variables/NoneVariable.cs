namespace Battlescript;

public class NoneVariable : Variable, IEquatable<NoneVariable>
{
    #region Equality

    public override bool Equals(object? obj) => obj is NoneVariable variable && Equals(variable);

    public bool Equals(NoneVariable? other) => other is not null;

    public override bool Equals(Variable? other) => other is NoneVariable variable && Equals(variable);

    public override int GetHashCode() => 45;

    public static bool operator ==(NoneVariable? left, NoneVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(NoneVariable? left, NoneVariable? right) => !(left == right);

    #endregion
}