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
    
    [TestFixture]
    public class InstructionParserStandardSeparators
    {
        [Test]
        public void HandlesSquareBrackets()
        {
            var lexer = new Lexer("[4, 'asdf']");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);
            
            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.SquareBrackets, 
                    new List<Instruction> 
                    {
                        new (0, 0, Consts.InstructionTypes.Number, 4),
                        new (0, 0, Consts.InstructionTypes.String, "asdf")
                    }
                )
            );
        }
        
        [Test]
        public void HandlesParens()
        {
            var lexer = new Lexer("(4, 'asdf')");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);
            
            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.Parens, 
                    new List<Instruction> 
                    {
                        new (0, 0, Consts.InstructionTypes.Number, 4),
                        new (0, 0, Consts.InstructionTypes.String, "asdf")
                    }
                )
            );
        }
    }
    
    [TestFixture]
    public class InstructionParserCurlyBraces
    {
        [Test]
        public void HandlesSetDefinition()
        {
            var lexer = new Lexer("{4, 'asdf'}");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);
            
            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.SetDefinition, 
                    new List<Instruction> 
                    {
                        new (0, 0, Consts.InstructionTypes.Number, 4),
                        new (0, 0, Consts.InstructionTypes.String, "asdf")
                    }
                )
            );
        }
        
        [Test]
        public void HandlesDictionaryDefinition()
        {
            var lexer = new Lexer("{4: 5, 6: 'asdf'}");
            var lexerResult = lexer.Run();
            var instructionParser = new InstructionParser();
            var instructionParserResult = instructionParser.Run(lexerResult);
            
            Assertions.AssertInstructionEqual(
                instructionParserResult,
                new Instruction(
                    0, 
                    0, 
                    Consts.InstructionTypes.DictionaryDefinition, 
                    new List<(Instruction Key, Instruction Value)> 
                    {
                        (
                            new (0, 0, Consts.InstructionTypes.Number, 4), 
                            new (0, 0, Consts.InstructionTypes.Number, 5)
                        ),
                        (
                            new (0, 0, Consts.InstructionTypes.Number, 6),
                            new (0, 0, Consts.InstructionTypes.String, "asdf")
                        )
                    }
                )
            );
        }
    }
}