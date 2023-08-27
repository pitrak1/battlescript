using BattleScript.Core;

namespace BattleScript.Tokens;
public class OperatorToken : Token
{
    public OperatorToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.Operator, value, line, column)
    { }
}