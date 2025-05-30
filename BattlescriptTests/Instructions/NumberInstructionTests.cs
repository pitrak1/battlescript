using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class NumberInstructionParse
    {
        [Test]
        public void HandlesSimpleNumbers()
        {
            var lexer = new Lexer("5");
            var lexerResult = lexer.Run();
            
            var expected = new NumberInstruction(5.0);
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class NumberInstructionInterpret
    {
        [Test]
        public void ReturnsNewNumberVariable()
        {
            var result = Runner.Run("x = 5");
            var expected = new Dictionary<string, Variable>
            {
                { "x", new NumberVariable(5) }
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
    }
}