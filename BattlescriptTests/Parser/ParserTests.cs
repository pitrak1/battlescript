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
                    Consts.InstructionTypes.Assignment, 
                    "=", 
                    null,
                    new (Consts.InstructionTypes.Variable, "x"),
                    new (Consts.InstructionTypes.Number, 5)
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
                    Consts.InstructionTypes.If, 
                    new Instruction(
                       Consts.InstructionTypes.Operation,
                       "<",
                       null,
                       new (Consts.InstructionTypes.Number, 5), 
                       new (Consts.InstructionTypes.Number, 6)
                    ), 
                    null,
                    null,
                    null,
                    new List<Instruction>
                    {
                        new(
                            Consts.InstructionTypes.Assignment,
                            "=",
                            null,
                            new (Consts.InstructionTypes.Variable, "x"),
                            new (Consts.InstructionTypes.Number, 5)
                        )
                    }
                )
            };
            
            Assertions.AssertInstructionListEqual(parserResult, expected);
        }
    }
}