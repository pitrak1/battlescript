namespace BattleScript; 

public class KeywordToken : Token {
    public KeywordToken(string value) : base(Consts.TokenTypes.Keyword, value) {}
}