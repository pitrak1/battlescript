using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class VariableInstructionParse
    {
        [Test]
        public void HandlesSimpleName()
        {
            var lexer = new Lexer("asdf");
            var lexerResult = lexer.Run();
            
            var expected = new VariableInstruction("asdf");
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}