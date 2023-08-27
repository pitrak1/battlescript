namespace BattleScript.Core;
public class BooleanToken : Token
{
    public BooleanToken(string value) : base(Consts.TokenTypes.Boolean, value) { }
}