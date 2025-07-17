using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class StringInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var expected = new StringInstruction("asdf");
            Assertions.AssertInputProducesParserOutput("'asdf'", expected);
        }
    }
    
    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var memory = Runner.Run("x = 'asdf'");
            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], new StringVariable("asdf"));
        }
    }
}