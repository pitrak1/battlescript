using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class StringInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void SingleQuoteStrings()
        {
            var expected = new StringInstruction("asdf");
            var result = Runner.Parse("'asdf'");
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void SingleQuoteStrings()
        {
            var (callStack, closure) = Runner.Run("x = 'asdf'");
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "asdf")));
        }
    }
}
