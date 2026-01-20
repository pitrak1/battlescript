using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class VariableInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void SimpleName()
        {
            var expected = new VariableInstruction("asdf");
            Assertions.AssertInputProducesParserOutput("asdf", expected);
        }
    }
}