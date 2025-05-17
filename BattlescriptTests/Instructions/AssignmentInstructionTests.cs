using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class AssignmentInstructionParse
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new NumberInstruction(6.0)
            );
            ParserAssertions.AssertInputProducesInstruction("x = 6", expected);
        }
    }
    
    [TestFixture]
    public class AssignmentInstructionInterpret
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            var expected = new Dictionary<string, Variable>
            {
                { "x", new NumberVariable(6.0) }
            };
            InterpreterAssertions.AssertInputProducesOutput("x = 6", [expected]);
        }
        
        [Test]
        public void HandlesAssignmentOperators()
        {
            var expected = new Dictionary<string, Variable>
            {
                { "x", new NumberVariable(8.0) }
            };
            InterpreterAssertions.AssertInputProducesOutput("x = 6\nx += 2", [expected]);
        }

        [Test]
        public void ReturnsAssignedVariable()
        {
            var scopes = Runner.Run("x = 6");
            InterpreterAssertions.AssertVariableEqual(scopes.First()["x"], new NumberVariable(6.0));
        }

        [Test]
        public void ThrowsErrorIfLeftHandSideIsNotVariable()
        {
            Assert.Throws<Exception>(() => Runner.Run("5 = 6"));
        }
    }
}