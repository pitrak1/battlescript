namespace BattleScript.Core;
public class WhitespaceToken : Token
{
    public WhitespaceToken() : base(Consts.TokenTypes.Whitespace, "") { }
}