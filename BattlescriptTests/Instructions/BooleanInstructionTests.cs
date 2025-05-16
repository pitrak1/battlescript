using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class BooleanInstructionParse
    {
        [Test]
        public void HandlesSimpleBooleanParsing()
        {
            var expected = new BooleanInstruction(false);
            ParserAssertions.AssertInputProducesInstruction("False", expected);
        }
    }
    
    [TestFixture]
    public class BooleanInstructionInterpret
    {
        [Test]
        public void ReturnsNewBooleanVariable()
        {
            var expected = new Dictionary<string, Variable>
            {
                { "x", new BooleanVariable(false) }
            };
            InterpreterAssertions.AssertInputProducesOutput("x = False", [expected]);
        }
    }
}