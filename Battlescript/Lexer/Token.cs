namespace Battlescript;

public class Token(Consts.TokenTypes type, string value, int? line = null, string? fileName = null, string? expression = null)
    : IEquatable<Token>
{
    public Consts.TokenTypes Type { get; set; } = type;
    public string Value { get; set; } = value;
    public int Line { get; set; } = line ?? -1;
    public string FileName { get; set; } = fileName ?? "main";
    public string Expression { get; set; } = expression ?? "";

    #region Equality

    public override bool Equals(object? obj) => obj is Token token && Equals(token);

    public bool Equals(Token? other) =>
        other is not null && Type == other.Type && Value == other.Value;

    public override int GetHashCode() => HashCode.Combine(Type, Value);

    public static bool operator ==(Token? left, Token? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Token? left, Token? right) => !(left == right);

    #endregion
}