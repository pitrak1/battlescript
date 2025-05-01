using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class IfInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new IfInstruction(new BooleanInstruction(true));
            ParserAssertions.AssertInputProducesInstruction("if True:", expected);
        }
    }
}