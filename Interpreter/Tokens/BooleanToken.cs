namespace BattleScript; 

public class BooleanToken : Token {
    public BooleanToken(string value) : base(Consts.TokenTypes.Boolean, value) {}
}