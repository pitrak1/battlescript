using Battlescript;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertInputProducesParserOutput(string input, Instruction? expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        
        Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
    }
}