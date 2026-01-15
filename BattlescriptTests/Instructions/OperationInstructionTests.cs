using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class OperationInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void BinaryOperations()
        {
            var input = "5 + 6";
            var expected = new OperationInstruction(
                operation: "+",
                left: new NumericInstruction(5),
                right: new NumericInstruction(6)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void UnaryOperations()
        {
            var input = "-6";
            var expected = new OperationInstruction(
                operation: "-",
                right: new NumericInstruction(6)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void BinaryOperationsWithParentheses()
        {
            var input = "4 * (5 + 5)";
            var expected = new OperationInstruction(
                operation: "*",
                left: new NumericInstruction(4),
                right: new OperationInstruction("+", new NumericInstruction(5), new NumericInstruction(5))
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var (callStack, closure) = Runner.Run("x = 5 + 6");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 11));
        }
    }
}