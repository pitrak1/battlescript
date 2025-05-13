using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class VariableInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleName()
        {
            var expected = new VariableInstruction("asdf");
            ParserAssertions.AssertInputProducesInstruction("asdf", expected);
        }
    }
}