using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class StringInstructionParse
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var expected = new StringInstruction("asdf");
            Assertions.AssertInputProducesParserOutput("'asdf'", expected);
        }
    }
    
    [TestFixture]
    public class StringInstructionInterpret
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var memory = Runner.Run("x = 'asdf'");
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(new StringVariable("asdf")));
        }
    }
}