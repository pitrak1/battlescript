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
            var expected = new NumberInstruction(5.0);
            ParserAssertions.AssertInputProducesInstruction("5", expected);
        }
    }
}