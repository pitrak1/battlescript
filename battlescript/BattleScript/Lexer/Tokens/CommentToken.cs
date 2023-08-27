using BattleScript.Core;

namespace BattleScript.Tokens;
public class CommentToken : Token
{
    public CommentToken(int line = -1, int column = -1) :
        base(Consts.TokenTypes.Comment, "", line, column)
    { }
}