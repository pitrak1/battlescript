namespace Battlescript;

public class BindingVariable(string value) : Variable, IEquatable<BindingVariable>
{
    public string Value { get; set; } = value;

    #region Equality

    public override bool Equals(object? obj) => obj is BindingVariable variable && Equals(variable);

    public bool Equals(BindingVariable? other) => other is not null && Value == other.Value;

    public override bool Equals(Variable? other) => other is BindingVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(BindingVariable? left, BindingVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(BindingVariable? left, BindingVariable? right) => !(left == right);

    #endregion
}
