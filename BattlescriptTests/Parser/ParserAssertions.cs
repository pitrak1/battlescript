using Battlescript;

namespace BattlescriptTests;

public static class ParserAssertions
{
    public static void AssertInputProducesInstruction(string input, Instruction expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();

        Assertions.AssertInstructionEqual(
            Instruction.Parse(lexerResult),
            expected
        );
    }
}