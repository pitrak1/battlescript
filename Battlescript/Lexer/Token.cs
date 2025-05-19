namespace Battlescript;

public class Token : IEquatable<Token>
{
    public Consts.TokenTypes Type { get; set; }
    public string Value { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }

    public Token(Consts.TokenTypes type, string value, int line = -1, int column = -1)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as Token);
    public bool Equals(Token token)
    {
        if (token is null) return false;
        if (ReferenceEquals(this, token)) return true;
        if (GetType() != token.GetType()) return false;
        return Type == token.Type && Value == token.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Type, Value);
    public static bool operator ==(Token left, Token right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(Token left, Token right) => !(left == right);
}