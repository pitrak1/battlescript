using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ReturnInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleValue()
        {
            var expected = new ReturnInstruction(new NumberInstruction(4));
            ParserAssertions.AssertInputProducesInstruction("return 4", expected);
        }
    }
}