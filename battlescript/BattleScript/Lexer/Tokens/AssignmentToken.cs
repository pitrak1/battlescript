using BattleScript.Core;

namespace BattleScript.Tokens;
public class AssignmentToken : Token
{
    public AssignmentToken(int line = -1, int column = -1) :
        base(Consts.TokenTypes.Assignment, "=", line, column)
    { }
}