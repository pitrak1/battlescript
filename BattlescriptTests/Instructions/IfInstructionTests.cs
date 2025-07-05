using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class IfInstructionParse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var lexer = new Lexer("if True:");
            var lexerResult = lexer.Run();
            
            var expected = new IfInstruction(new ConstantInstruction("True"));
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class IfInstructionInterpret
    {
        [Test]
        public void RunsCodeIfConditionIsTrue()
        {
            var memory = Runner.Run("x = 3\nif True:\n\tx = 5");
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(new IntegerVariable(5)));
        }
        
        [Test]
        public void DoesNotRunCodeIfConditionIsFalse()
        {
            var memory = Runner.Run("x = 3\nif False:\n\tx = 5");
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(new IntegerVariable(3)));
        }
    }
}