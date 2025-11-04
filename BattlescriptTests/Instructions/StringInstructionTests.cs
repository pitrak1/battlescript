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
            var (callStack, closure) = Runner.Run("x = 'asdf'");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.String, "asdf"));
        }
    }
}