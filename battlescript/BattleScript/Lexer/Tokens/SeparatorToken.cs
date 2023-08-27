using BattleScript.Core;

namespace BattleScript.Tokens;
public class SeparatorToken : Token
{
    public SeparatorToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.Separator, value, line, column)
    { }
}