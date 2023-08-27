namespace BattleScript.Core;
public class KeywordToken : Token
{
    public KeywordToken(string value) : base(Consts.TokenTypes.Keyword, value) { }
}