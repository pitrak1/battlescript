using Battlescript;

namespace BattlescriptTests.Instructions;

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
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
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
                    new ArrayInstruction([new StringInstruction("i")], separator: "[")),
                right: new IntegerInstruction(6)
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
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
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }

        [Test]
        public void HandlesParenthesis()
        {
            var lexer = new Lexer("4 * (5 + 5)");
            var lexerResult = lexer.Run();
            
            var expected = new OperationInstruction(
                operation: "*",
                left: new IntegerInstruction(4),
                right: new OperationInstruction("+", new IntegerInstruction(5), new IntegerInstruction(5))
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class OperationInstructionInterpret
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var memory = Runner.Run("x = 5 + 6");
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(new IntegerVariable(11)));
        }
    }
}