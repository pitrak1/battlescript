using BattleScript.Core;

namespace BattleScript.Tokens;
public class SemicolonToken : Token
{
    public SemicolonToken(int line = -1, int column = -1) :
        base(Consts.TokenTypes.Semicolon, ";", line, column)
    { }
}