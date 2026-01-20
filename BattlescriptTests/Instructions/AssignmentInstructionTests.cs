using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class AssignmentInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void Value()
        {
            var input = "x = 6";
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new NumericInstruction(6)
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void Expression()
        {
            var input = "x = 6 + 5";
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new OperationInstruction("+", new NumericInstruction(6), new NumericInstruction(5))
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class Interpret
    {
        [Test]
        public void SimpleAssignments()
        {
            var (callStack, closure) = Runner.Run("x = 6");
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void AssignmentOperators()
        {
            var (callStack, closure) = Runner.Run("x = 6\nx += 2");
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 8);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }
}