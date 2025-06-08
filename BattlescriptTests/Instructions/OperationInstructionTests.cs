using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class OperationInstructionParse
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var lexer = new Lexer("5 + 6");
            var lexerResult = lexer.Run();
            
            var expected = new OperationInstruction(
                operation: "+",
                left: new IntegerInstruction(5),
                right: new IntegerInstruction(6)
            );
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesBinaryOperationsWithExpressions()
        {
            var lexer = new Lexer("x.i + 6");
            var lexerResult = lexer.Run();
            
            var expected = new OperationInstruction(
                operation: "+",
                left: new VariableInstruction(
                    "x", 
                    new SquareBracketsInstruction(
                        [new StringInstruction("i")])),
                right: new IntegerInstruction(6)
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesUnaryOperators()
        {
            var lexer = new Lexer("~6");
            var lexerResult = lexer.Run();
            
            var expected = new OperationInstruction(
                operation: "~",
                right: new IntegerInstruction(6)
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class OperationInstructionInterpret
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var result = Runner.Run("x = 5 + 6");
            var expected = new Dictionary<string, Variable>()
            {
                ["x"] = new IntegerVariable(11)
            };
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}