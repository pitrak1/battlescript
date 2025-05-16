using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ReturnInstructionParse
    {
        [Test]
        public void HandlesSimpleValue()
        {
            var expected = new ReturnInstruction(new NumberInstruction(4));
            ParserAssertions.AssertInputProducesInstruction("return 4", expected);
        }
    }
}