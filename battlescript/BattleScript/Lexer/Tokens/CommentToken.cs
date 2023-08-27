using BattleScript.Core;

namespace BattleScript.Tokens;
public class CommentToken : Token
{
    public CommentToken() : base(Consts.TokenTypes.Comment, "") { }
}