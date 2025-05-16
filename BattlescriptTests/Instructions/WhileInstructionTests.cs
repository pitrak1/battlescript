using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class WhileInstructionParse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new WhileInstruction(new BooleanInstruction(true));
            ParserAssertions.AssertInputProducesInstruction("while True:", expected);
        }
    }
}