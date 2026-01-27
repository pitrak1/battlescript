namespace Battlescript;

public class NumericVariable : Variable, IEquatable<NumericVariable>
{
    private dynamic _value;
    public dynamic Value
    {
        get => _value;
        set
        {
            if (value is not double && value is not int)
            {
                throw new Exception("Wrong type for numeric instruction");
            }
            else
            {
                _value = value;
            }
        }
    }

    public NumericVariable(dynamic value)
    {
        _value = value;
    }

    public override Variable Copy()
    {
        return new NumericVariable(_value);
    }

    #region Equality

    public override bool Equals(object? obj) => obj is NumericVariable variable && Equals(variable);

    public bool Equals(NumericVariable? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(Variable? other) => other is NumericVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(NumericVariable? left, NumericVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(NumericVariable? left, NumericVariable? right) => !(left == right);

    #endregion
}