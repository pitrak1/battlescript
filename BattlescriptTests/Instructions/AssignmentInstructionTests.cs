using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class AssignmentInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new NumericInstruction(6)
            );
            Assertions.AssertInputProducesParserOutput("x = 6", expected);
        }
        
        [Test]
        public void HandlesExpressionAssignments()
        {
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new OperationInstruction("+", new NumericInstruction(6), new NumericInstruction(5))
            );
            Assertions.AssertInputProducesParserOutput("x = 6 + 5", expected);
        }
    }
    
    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            var memory = Runner.Run("x = 6");
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 6);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesAssignmentOperators()
        {
            var memory = Runner.Run("x = 6\nx += 2");
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 8);
            Assertions.AssertVariable(memory, "x", expected);
        }

        [Test]
        public void ReturnsAssignedVariable()
        {
            var memory = Runner.Run("x = 6");
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 6);
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
}