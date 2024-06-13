using BattleScript.Core;

namespace BattleScript.Tokens;

public class Token
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
}