using Battlescript;

namespace BattlescriptTests.InstructionsTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class StringInstructionParse
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var lexer = new Lexer("'asdf'");
            var lexerResult = lexer.Run();
            
            var expected = new StringInstruction("asdf");
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
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