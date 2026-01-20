namespace Battlescript;

public class ConstantVariable : Variable, IEquatable<ConstantVariable>
{
    public Consts.Constants Value { get; set; } = Consts.Constants.None;

    #region Equality

    public override bool Equals(object? obj) => obj is ConstantVariable variable && Equals(variable);

    public bool Equals(ConstantVariable? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(Variable? other) => other is ConstantVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(ConstantVariable? left, ConstantVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ConstantVariable? left, ConstantVariable? right) => !(left == right);

    #endregion
}