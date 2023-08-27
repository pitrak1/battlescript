using BattleScript.Core;

namespace BattleScript.Tokens;
public class KeywordToken : Token
{
    public KeywordToken(string value) : base(Consts.TokenTypes.Keyword, value) { }
}