using BattleScript.Core;

namespace BattleScript.Tokens;
public class IdentifierToken : Token
{
    public IdentifierToken(string value) : base(Consts.TokenTypes.Identifier, value) { }
}