using BattleScript.Core;

namespace BattleScript.Tokens;
public class NumberToken : Token
{
    public NumberToken(string value) : base(Consts.TokenTypes.Number, value) { }
}