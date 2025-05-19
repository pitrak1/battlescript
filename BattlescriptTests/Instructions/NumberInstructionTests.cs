using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class NumberInstructionParse
    {
        [Test]
        public void HandlesSimpleNumbers()
        {
            var lexer = new Lexer("5");
            var lexerResult = lexer.Run();
            
            var expected = new NumberInstruction(5.0);
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}