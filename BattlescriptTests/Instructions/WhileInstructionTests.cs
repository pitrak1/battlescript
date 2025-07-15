using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class WhileInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new WhileInstruction(new ConstantInstruction("True"));
            Assertions.AssertInputProducesParserOutput("while True:", expected);
        }
    }
}