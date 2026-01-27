namespace Battlescript;

public class StringVariable : Variable, IEquatable<StringVariable>
{
    public string Value { get; set; }

    public StringVariable(string? value = null)
    {
        Value = value ?? "";
    }

    public SequenceVariable ToSequence() =>
        new SequenceVariable(Value.Select(c => new StringVariable(c.ToString())).Cast<Variable>().ToList());
    
    public SequenceVariable ToBtlSequence() =>
        new SequenceVariable(Value.Select(c => BtlTypes.Create(BtlTypes.Types.String, c.ToString())).ToList());
    
    public override Variable Copy() => new StringVariable(Value);

    #region Equality

    public override bool Equals(object? obj) => obj is StringVariable variable && Equals(variable);

    public bool Equals(StringVariable? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(Variable? other) => other is StringVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(StringVariable? left, StringVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(StringVariable? left, StringVariable? right) => !(left == right);

    #endregion
}