using BattleScript.Core;

namespace BattleScript.Tokens;
public class StringToken : Token
{
    public StringToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.String, value, line, column)
    { }
}