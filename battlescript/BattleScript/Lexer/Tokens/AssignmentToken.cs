namespace BattleScript.Core;
public class AssignmentToken : Token
{
    public AssignmentToken() : base(Consts.TokenTypes.Assignment, "=") { }
}