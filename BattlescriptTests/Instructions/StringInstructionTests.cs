using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class StringInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void SingleQuoteStrings()
        {
            var expected = new StringInstruction("asdf");
            Assertions.AssertInputProducesParserOutput("'asdf'", expected);
        }
    }
    
    [TestFixture]
    public class Interpret
    {
        [Test]
        public void SingleQuoteStrings()
        {
            var (callStack, closure) = Runner.Run("x = 'asdf'");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.String, "asdf"));
        }
    }
}