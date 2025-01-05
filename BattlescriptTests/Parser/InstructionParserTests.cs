using Battlescript;

namespace BattlescriptTests;

public static class InstructionParserTests
{
    [TestFixture]
    public class InstructionParserLiterals
    {
        [Test]
        public void HandlesNumbers()
        {
            var lexer = new Lexer("5");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);

            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(0, 0, Consts.InstructionTypes.Number, 5.0)
            );
        }
        
        [Test]
        public void HandlesStrings()
        {
            var lexer = new Lexer("'asdf'");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);

            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(0, 0, Consts.InstructionTypes.String, "asdf")
            );
        }
        
        [Test]
        public void HandlesBooleans()
        {
            var lexer = new Lexer("False");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);

            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(0, 0, Consts.InstructionTypes.Boolean, false)
            );
        }
    }

    [TestFixture]
    public class InstructionParserOperations
    {
        [Test]
        public void HandlesOperations()
        {
            var lexer = new Lexer("5 + 6");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);

            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.Operation, 
                    "+", 
                    new Instruction(0, 0, Consts.InstructionTypes.Number, 5.0),
                    new Instruction(0, 0, Consts.InstructionTypes.Number, 6.0)
                )
            );
        }
    }
    
    [TestFixture]
    public class InstructionParserAssignments
    {
        [Test]
        public void HandlesAssignments()
        {
            // This is nonsensical, but is an easy example for this test *shrug*
            var lexer = new Lexer("5 = 6");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);

            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.Assignment, 
                    "=", 
                    new Instruction(0, 0, Consts.InstructionTypes.Number, 5.0),
                    new Instruction(0, 0, Consts.InstructionTypes.Number, 6.0)
                )
            );
        }
    }
}