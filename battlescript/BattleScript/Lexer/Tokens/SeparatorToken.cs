namespace BattleScript.Core;
public class SeparatorToken : Token
{
    public SeparatorToken(string value) : base(Consts.TokenTypes.Separator, value) { }
}