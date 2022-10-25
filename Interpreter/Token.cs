namespace BattleScript;

public class Token {
    public Consts.TokenTypes Type { get; }
    public string Value { get; }
    public int? Line { get; set; }
    public int? Column { get; set; }

    public Token(Consts.TokenTypes type, string value, int? line = null, int? column = null) {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }

    public void SetDebugInfo(int line, int column) {
        Line = line;
        Column = column;
    }
    
    public static bool operator ==(Token a, Token b) {
        return a.Type == b.Type && a.Value == b.Value;
    }

    public static bool operator !=(Token a, Token b) {
        return a.Type != b.Type || a.Value != b.Value;
    }
}