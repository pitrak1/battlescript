using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class VariableInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void SimpleName()
        {
            var expected = new VariableInstruction("asdf");
            var result = Runner.Parse("asdf");
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}
