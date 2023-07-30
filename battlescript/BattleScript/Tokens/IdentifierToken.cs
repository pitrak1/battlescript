namespace BattleScript.Core;
public class IdentifierToken : Token
{
    public IdentifierToken(string value) : base(Consts.TokenTypes.Identifier, value) { }
}