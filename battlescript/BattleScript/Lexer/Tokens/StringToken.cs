namespace BattleScript.Core;
public class StringToken : Token
{
    public StringToken(string value) : base(Consts.TokenTypes.String, value) { }
}