using BattleScript.Core;

namespace BattleScript.Tokens;
public class NumberToken : Token
{
    public NumberToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.Number, value, line, column)
    { }
}