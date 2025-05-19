using Battlescript;

namespace BattlescriptTests;

public static class LexerAssertions
{
    public static void AssertInputProducesOutput(string input, List<Token> expected)
    {
        var lexer = new Lexer(input);
        var result = lexer.Run();
        Assertions.AssertTokenListEqual(result, expected);
    }
}