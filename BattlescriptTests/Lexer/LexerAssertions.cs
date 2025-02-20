using Battlescript;

namespace BattlescriptTests;

public static class LexerAssertions
{
    public static void AssertInputProducesOutput(string input, Consts.TokenTypes type, string value)
    {
        var lexer = new Lexer(input);
        var result = lexer.Run();
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Type, Is.EqualTo(type));
        Assert.That(result[0].Value, Is.EqualTo(value));
    }

    public static void AssertInputProducesOutputList(string input, List<Token> expected)
    {
        var lexer = new Lexer(input);
        var result = lexer.Run();
        Assertions.AssertTokenListEqual(result, expected);
    }
}