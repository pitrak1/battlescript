using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class IfInstructionParse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new IfInstruction(new BooleanInstruction(true));
            ParserAssertions.AssertInputProducesInstruction("if True:", expected);
        }
    }
}