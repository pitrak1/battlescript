using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ParensInstructionParse
    {
        [Test]
        public void HandlesSimpleValueLists()
        {
            var expected = new ParensInstruction(
                [
                    new NumberInstruction(4),
                    new StringInstruction("asdf")
                ]
            );
            ParserAssertions.AssertInputProducesInstruction("(4, 'asdf')", expected);
        }
    }
}