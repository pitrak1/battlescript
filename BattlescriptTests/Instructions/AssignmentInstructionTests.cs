using Battlescript;

namespace BattlescriptTests.Instructions;

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
                right: new NumericInstruction(6)
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesExpressionAssignments()
        {
            var lexer = new Lexer("x = 6 + 5");
            var lexerResult = lexer.Run();
            
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new VariableInstruction("x"),
                right: new OperationInstruction("+", new NumericInstruction(5), new NumericInstruction(6))
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class AssignmentInstructionInterpret
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            var memory = Runner.Run("x = 6");
            Assert.That(memory.Scopes.First()["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6)));
        }
        
        [Test]
        public void HandlesAssignmentOperators()
        {
            var memory = Runner.Run("x = 6\nx += 2");
            Assert.That(memory.Scopes.First()["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8)));
        }

        [Test]
        public void ReturnsAssignedVariable()
        {
            var memory = Runner.Run("x = 6");
            Assert.That(memory.Scopes.First()["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6)));
        }
    }
}