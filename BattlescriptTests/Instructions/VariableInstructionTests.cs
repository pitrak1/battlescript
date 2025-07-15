using Battlescript;

namespace BattlescriptTests.Instructions;

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
            Assertions.AssertInputProducesParserOutput("asdf", expected);
        }
    }
}