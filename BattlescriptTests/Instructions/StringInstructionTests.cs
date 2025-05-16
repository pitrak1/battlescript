using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class StringInstructionParse
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var expected = new StringInstruction("asdf");
            ParserAssertions.AssertInputProducesInstruction("'asdf'", expected);
        }
    }
}