namespace BattleScript; 

public class WhitespaceToken : Token {
    public WhitespaceToken() : base(Consts.TokenTypes.Whitespace, "") {}
}