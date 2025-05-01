using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class WhileInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new WhileInstruction(new BooleanInstruction(true));
            ParserAssertions.AssertInputProducesInstruction("while True:", expected);
        }
    }
}