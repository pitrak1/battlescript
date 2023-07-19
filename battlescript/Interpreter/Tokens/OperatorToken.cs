namespace BattleScript; 

public class OperatorToken : Token {
    public OperatorToken(string value) : base(Consts.TokenTypes.Operator, value) {}
}