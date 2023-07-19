namespace BattleScript; 

public class AssignmentToken : Token {
    public AssignmentToken() : base(Consts.TokenTypes.Assignment, "=") {}
}