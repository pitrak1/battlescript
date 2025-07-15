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
            var expected = new OperationInstruction(
                operation: "+",
                left: new NumericInstruction(5),
                right: new NumericInstruction(6)
            );
            Assertions.AssertInputProducesParserOutput("5 + 6", expected);
        }
        
        [Test]
        public void HandlesBinaryOperationsWithExpressions()
        {
            var expected = new OperationInstruction(
                operation: "+",
                left: new VariableInstruction(
                    "x", 
                    new ArrayInstruction([new StringInstruction("i")], separator: "[")),
                right: new NumericInstruction(6)
            );
            Assertions.AssertInputProducesParserOutput("x.i + 6", expected);
        }
        
        [Test]
        public void HandlesUnaryOperators()
        {
            var expected = new OperationInstruction(
                operation: "-",
                right: new NumericInstruction(6)
            );
            Assertions.AssertInputProducesParserOutput("-6", expected);
        }

        [Test]
        public void HandlesParenthesis()
        {
            var expected = new OperationInstruction(
                operation: "*",
                left: new NumericInstruction(4),
                right: new OperationInstruction("+", new NumericInstruction(5), new NumericInstruction(5))
            );
            Assertions.AssertInputProducesParserOutput("4 * (5 + 5)", expected);
        }
    }
    
    [TestFixture]
    public class OperationInstructionInterpret
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var memory = Runner.Run("x = 5 + 6");
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 11)));
        }
    }
}