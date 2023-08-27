using BattleScript.Core;

namespace BattleScript.Tokens;
public class StringToken : Token
{
    public StringToken(string value) : base(Consts.TokenTypes.String, value) { }
}