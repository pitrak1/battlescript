using BattleScript.Core;

namespace BattleScript.Tokens;
public class BooleanToken : Token
{
    public BooleanToken(string value) : base(Consts.TokenTypes.Boolean, value) { }
}