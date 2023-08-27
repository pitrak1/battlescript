using BattleScript.Core;

namespace BattleScript.Tokens;
public class KeywordToken : Token
{
    public KeywordToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.Keyword, value, line, column)
    { }
}