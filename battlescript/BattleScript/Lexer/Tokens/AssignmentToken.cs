using BattleScript.Core;

namespace BattleScript.Tokens;
public class AssignmentToken : Token
{
    public AssignmentToken() : base(Consts.TokenTypes.Assignment, "=") { }
}