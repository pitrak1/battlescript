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
            var lexer = new Lexer("x = 6");
            var lexerResult = lexer.Run();
            
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new IntegerInstruction(6)
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesExpressionAssignments()
        {
            var lexer = new Lexer("x = 6 + 5");
            var lexerResult = lexer.Run();
            
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new OperationInstruction("+", new IntegerInstruction(5), new IntegerInstruction(6))
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class AssignmentInstructionInterpret
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            var result = Runner.Run("x = 6");
            var expected = new Dictionary<string, Variable>
            {
                { "x", new IntegerVariable(6) }
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesAssignmentOperators()
        {
            var result = Runner.Run("x = 6\nx += 2");
            var expected = new Dictionary<string, Variable>
            {
                { "x", new IntegerVariable(8) }
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }

        [Test]
        public void ReturnsAssignedVariable()
        {
            var scopes = Runner.Run("x = 6");
            Assert.That(scopes.First()["x"], Is.EqualTo(new IntegerVariable(6)));
        }

        [Test]
        public void ThrowsErrorIfLeftHandSideIsNotVariable()
        {
            Assert.Throws<Exception>(() => Runner.Run("5 = 6"));
        }
    }
}