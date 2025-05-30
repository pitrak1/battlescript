using Battlescript;

namespace BattlescriptTests;

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
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class StringInstructionInterpret
    {
        [Test]
        public void HandlesSingleQuoteStrings()
        {
            var results = Runner.Run("x = 'asdf'");
            Assert.That(results[0]["x"], Is.EqualTo(new StringVariable("asdf")));
        }
    }
}