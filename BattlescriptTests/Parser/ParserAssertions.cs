using Battlescript;

namespace BattlescriptTests;

public static class ParserAssertions
{
    public static void AssertInputProducesInstruction(string input, Instruction expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        var instructionParser = new InstructionParser();
        var instructionParserResult = instructionParser.Run(lexerResult);

        Assertions.AssertInstructionEqual(
            instructionParserResult,
            expected
        );
    }
}