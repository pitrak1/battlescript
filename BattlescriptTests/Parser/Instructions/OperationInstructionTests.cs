using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class OperationInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesBinaryOperations()
        {
            var expected = new OperationInstruction(
                operation: "+",
                left: new NumberInstruction(5.0),
                right: new NumberInstruction(6.0)
            );
            ParserAssertions.AssertInputProducesInstruction("5 + 6", expected);
        }
        
        [Test]
        public void HandlesUnaryOperators()
        {
            var expected = new OperationInstruction(
                operation: "~",
                right: new NumberInstruction(6.0)
            );
            ParserAssertions.AssertInputProducesInstruction("~6", expected);
        }
    }
}