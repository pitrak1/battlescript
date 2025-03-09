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
                new (
                    type: Consts.InstructionTypes.Assignment, 
                    operation: "=", 
                    left: new (type: Consts.InstructionTypes.Variable, name: "x"),
                    right: new (Consts.InstructionTypes.Number, 5)
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
                new (
                    type: Consts.InstructionTypes.If, 
                    value: new Instruction(
                       type: Consts.InstructionTypes.Operation,
                       operation: "<",
                       left: new (Consts.InstructionTypes.Number, 5), 
                       right: new (Consts.InstructionTypes.Number, 6)
                    ), 
                    instructions: new List<Instruction>
                    {
                        new(
                            type: Consts.InstructionTypes.Assignment,
                            operation: "=",
                            left: new (type: Consts.InstructionTypes.Variable, name: "x"),
                            right: new (Consts.InstructionTypes.Number, 5)
                        )
                    }
                )
            };
            
            Assertions.AssertInstructionListEqual(parserResult, expected);
        }
        
        [Test]
        public void HandlesInstructionsBeforeConditionalInstructionBlock()
        {
            var lexer = new Lexer("y = 7\nif 5 < 6:\n\tx = 5");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();

            var expected = new List<Instruction>
            {
                new(
                    type: Consts.InstructionTypes.Assignment,
                    operation: "=",
                    left: new (type: Consts.InstructionTypes.Variable, name: "y"),
                    right: new (Consts.InstructionTypes.Number, 7)
                ),
                new (
                    type: Consts.InstructionTypes.If, 
                    value: new (
                        type: Consts.InstructionTypes.Operation,
                        operation: "<",
                        left: new (Consts.InstructionTypes.Number, 5), 
                        right: new (Consts.InstructionTypes.Number, 6)
                    ), 
                    instructions: new List<Instruction>
                    {
                        new(
                            type: Consts.InstructionTypes.Assignment,
                            operation: "=",
                            left: new (type: Consts.InstructionTypes.Variable, name: "x"),
                            right: new (Consts.InstructionTypes.Number, 5)
                        )
                    }
                )
            };
            
            Assertions.AssertInstructionListEqual(parserResult, expected);
        }
        
        [Test]
        public void HandlesInstructionsAfterConditionalInstructionBlock()
        {
            var lexer = new Lexer("if 5 < 6:\n\tx = 5\ny = 7");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();

            var expected = new List<Instruction>
            {
                new (
                    type: Consts.InstructionTypes.If, 
                    value: new (
                        type: Consts.InstructionTypes.Operation,
                        operation: "<",
                        left: new (Consts.InstructionTypes.Number, 5), 
                        right: new (Consts.InstructionTypes.Number, 6)
                    ),
                    instructions: new List<Instruction>
                    {
                        new(
                            type: Consts.InstructionTypes.Assignment,
                            operation: "=",
                            left: new (type: Consts.InstructionTypes.Variable, name: "x"),
                            right: new (Consts.InstructionTypes.Number, 5)
                        )
                    }
                ),
                new(
                    type: Consts.InstructionTypes.Assignment,
                    operation: "=",
                    left: new (type: Consts.InstructionTypes.Variable, name: "y"),
                    right: new (Consts.InstructionTypes.Number, 7)
                )
            };
            
            Assertions.AssertInstructionListEqual(parserResult, expected);
        }
    }
}