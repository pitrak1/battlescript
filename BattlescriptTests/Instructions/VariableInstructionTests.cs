using Battlescript;

namespace BattlescriptTests.Instructions;

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
            Assertions.AssertInputProducesParserOutput("asdf", expected);
        }
    }
}