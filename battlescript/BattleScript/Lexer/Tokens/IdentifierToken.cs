using BattleScript.Core;

namespace BattleScript.Tokens;
public class IdentifierToken : Token
{
    public IdentifierToken(string value, int line = -1, int column = -1) :
        base(Consts.TokenTypes.Identifier, value, line, column)
    { }
}