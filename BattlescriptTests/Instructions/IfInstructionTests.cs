using Battlescript;

namespace BattlescriptTests;

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
            
            var expected = new IfInstruction(new BooleanInstruction(true));
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class IfInstructionInterpret
    {
        [Test]
        public void RunsCodeIfConditionIsTrue()
        {
            var result = Runner.Run("x = 3\nif True:\n\tx = 5");
            var expected = new Dictionary<string, Variable>()
            {
                ["x"] = new IntegerVariable(5)
            };
            
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void DoesNotRunCodeIfConditionIsFalse()
        {
            var result = Runner.Run("x = 3\nif False:\n\tx = 5");
            var expected = new Dictionary<string, Variable>()
            {
                ["x"] = new IntegerVariable(3)
            };
            
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}