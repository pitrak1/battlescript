using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class WhileInstructionParse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var expected = new WhileInstruction(new ConstantInstruction("True"));
            Assertions.AssertInputProducesParserOutput("while True:", expected);
        }
    }
}