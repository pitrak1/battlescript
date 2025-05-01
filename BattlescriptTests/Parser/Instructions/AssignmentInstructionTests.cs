using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class AssignmentInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleAssignments()
        {
            // This is nonsensical, but is an easy example for this test *shrug*
            var expected = new AssignmentInstruction(
                operation: "=",
                left: new NumberInstruction(5.0),
                right: new NumberInstruction(6.0)
            );
            ParserAssertions.AssertInputProducesInstruction("5 = 6", expected);
        }
    }
}