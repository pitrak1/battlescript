using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class BooleanInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var expected = new BooleanInstruction(false);
            ParserAssertions.AssertInputProducesInstruction("False", expected);
        }
    }
}