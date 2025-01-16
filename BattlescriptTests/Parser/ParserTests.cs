using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ParserTests
{
    [TestFixture]
    public class ParserBasics
    {
        [Test]
        public void HandlesSingleInstruction()
        {
            var lexer = new Lexer("x = 5");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();

            var expected = new List<Instruction>
            {
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.Assignment, 
                    "=", 
                    null,
                    new Instruction(
                        0,
                        0,
                        Consts.InstructionTypes.Variable,
                        "x"
                    ),
                    new Instruction(
                        0,
                        0,
                        Consts.InstructionTypes.Number,
                        5
                    )
                )
            };
            
            Assertions.AssertInstructionListEqual(parserResult, expected);
        }
    }
}