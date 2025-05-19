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
}