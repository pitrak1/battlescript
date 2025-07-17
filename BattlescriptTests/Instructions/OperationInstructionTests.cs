using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class OperationInstructionTests
{
    [TestFixture]
    public class Parse
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
                    new MemberInstruction("i")),
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
    public class Interpret
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var memory = Runner.Run("x = 5 + 6");
            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 11));
        }
    }
}