namespace BattleScript.Core;
public class SemicolonToken : Token
{
    public SemicolonToken() : base(Consts.TokenTypes.Semicolon, ";") { }
}