using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class IntegerInstructionParse
    {
        [Test]
        public void HandlesSimpleNumbers()
        {
            var lexer = new Lexer("5");
            var lexerResult = lexer.Run();
            
            var expected = new IntegerInstruction(5);
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class IntegerInstructionInterpret
    {
        [Test]
        public void ReturnsNewNumberVariable()
        {
            var result = Runner.Run("x = 5");
            var expected = new Dictionary<string, Variable>
            {
                { "x", new IntegerVariable(5) }
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
    }
}