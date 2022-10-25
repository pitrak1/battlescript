namespace BattleScript;

public class Token {
    public Consts.TokenTypes Type;
    public string Value;
    public int? Line;
    public int? Column;

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