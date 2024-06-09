using System.Text.Json;
using Newtonsoft.Json;
using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Instructions;

namespace BattleScript.Tests;

public class Assertions
{
    public static void AssertTokens(List<Token> tokens, List<Token> expectedTokens)
    {
        for (int i = 0; i < expectedTokens.Count; i++)
        {
            AssertToken(tokens[i], expectedTokens[i]);
        }
    }

    private static void AssertToken(Token token, Token expected)
    {
        Assert.That(token.Type, Is.EqualTo(expected.Type));
        Assert.That(token.Value, Is.EqualTo(expected.Value));
    }

    public static void AssertScope(
        Dictionary<string, Variable> scope,
        Dictionary<string, Variable> expected
    )
    {
        Assert.That(scope, Is.EquivalentTo(expected));
    }
}