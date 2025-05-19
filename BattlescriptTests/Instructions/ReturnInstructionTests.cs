using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ReturnInstructionParse
    {
        [Test]
        public void HandlesSimpleValue()
        {
            var lexer = new Lexer("return 4");
            var lexerResult = lexer.Run();
            
            var expected = new ReturnInstruction(new NumberInstruction(4));
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}