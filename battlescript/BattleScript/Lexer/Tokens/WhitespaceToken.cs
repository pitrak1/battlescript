using BattleScript.Core;

namespace BattleScript.Tokens;
public class WhitespaceToken : Token
{
    public WhitespaceToken() : base(Consts.TokenTypes.Whitespace, "") { }
}