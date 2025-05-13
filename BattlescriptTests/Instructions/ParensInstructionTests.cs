using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ParensInstructionTests
{
    [TestFixture]
    public class Parse
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