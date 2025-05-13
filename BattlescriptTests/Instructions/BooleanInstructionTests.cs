using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class BooleanInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleBooleanParsing()
        {
            var expected = new BooleanInstruction(false);
            ParserAssertions.AssertInputProducesInstruction("False", expected);
        }
    }
    
    [TestFixture]
    public class Interpret
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