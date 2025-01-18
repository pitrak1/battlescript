using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ParserTests
{
    [TestFixture]
    public class Basics
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

        [Test]
        public void HandlesConditionalInstructionBlocks()
        {
            var lexer = new Lexer("if 5 < 6:\n\tx = 5");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();

            var expected = new List<Instruction>
            {
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.If, 
                    new Instruction(
                       0,
                       0,
                       Consts.InstructionTypes.Operation,
                       "<",
                       null,
                       new Instruction(
                           0,
                           0,
                           Consts.InstructionTypes.Number,
                           5
                        ),
                       new Instruction(
                            0,
                            0,
                            Consts.InstructionTypes.Number,
                            6
                        )
                    ), 
                    null,
                    null,
                    null,
                    new List<Instruction>
                    {
                        new(
                            0,
                            0,
                            Consts.InstructionTypes.Assignment,
                            "=",
                            null,
                            new (
                                0,
                                0,
                                Consts.InstructionTypes.Variable,
                                "x"
                            ),
                            new (
                                0,
                                0,
                                Consts.InstructionTypes.Number,
                                5
                            )
                        )
                    }
                )
            };
            
            Assertions.AssertInstructionListEqual(parserResult, expected);
        }
    }
}