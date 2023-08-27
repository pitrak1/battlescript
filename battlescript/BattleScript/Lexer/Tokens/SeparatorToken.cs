using BattleScript.Core;

namespace BattleScript.Tokens;
public class SeparatorToken : Token
{
    public SeparatorToken(string value) : base(Consts.TokenTypes.Separator, value) { }
}