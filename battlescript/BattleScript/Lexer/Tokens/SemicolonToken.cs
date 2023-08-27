using BattleScript.Core;

namespace BattleScript.Tokens;
public class SemicolonToken : Token
{
    public SemicolonToken() : base(Consts.TokenTypes.Semicolon, ";") { }
}