using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class StringInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var expected = new StringInstruction("asdf");
            ParserAssertions.AssertInputProducesInstruction("'asdf'", expected);
        }
    }
}