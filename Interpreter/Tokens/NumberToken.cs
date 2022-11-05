namespace BattleScript; 

public class NumberToken : Token {
    public NumberToken(string value) : base(Consts.TokenTypes.Number, value) {}
}