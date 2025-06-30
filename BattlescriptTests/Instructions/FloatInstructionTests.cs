using Battlescript;

namespace BattlescriptTests.InstructionsTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class FloatInstructionParse
    {
        [Test]
        public void HandlesSimpleNumbers()
        {
            var lexer = new Lexer("5.8");
            var lexerResult = lexer.Run();
            
            var expected = new FloatInstruction(5.8);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class FloatInstructionInterpret
    {
        [Test]
        public void ReturnsNewNumberVariable()
        {
            var memory = Runner.Run("x = 5.4");
            var expected = new Dictionary<string, Variable>
            {
                { "x", new FloatVariable(5.4) }
            };
            Assert.That(memory.Scopes.First(), Is.EquivalentTo(expected));
        }
    }
}