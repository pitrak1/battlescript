using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class VariableInstructionParse
    {
        [Test]
        public void HandlesSimpleName()
        {
            var expected = new VariableInstruction("asdf");
            ParserAssertions.AssertInputProducesInstruction("asdf", expected);
        }
    }
}