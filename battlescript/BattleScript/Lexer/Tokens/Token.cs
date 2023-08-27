namespace BattleScript.Core;

public class Token
{
    public Consts.TokenTypes Type { get; set; }
    public string Value { get; set; }
    public int? Line { get; set; }
    public int? Column { get; set; }

    public Token(Consts.TokenTypes type, string value)
    {
        Type = type;
        Value = value;
    }

    public Token SetDebugInfo(int? line, int? column)
    {
        Line = line;
        Column = column;
        return this;
    }
}