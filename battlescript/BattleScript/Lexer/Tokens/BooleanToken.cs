using BattleScript.Core;

namespace BattleScript.Tokens;
public class BooleanToken : Token
{
    public BooleanToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.Boolean, value, line, column)
    { }
}