using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class NumberInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleNumbers()
        {
            var expected = new NumberInstruction(5.0);
            ParserAssertions.AssertInputProducesInstruction("5", expected);
        }
    }
}