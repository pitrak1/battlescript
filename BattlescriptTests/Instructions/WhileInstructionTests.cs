using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class WhileInstructionParse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var lexer = new Lexer("while True:");
            var lexerResult = lexer.Run();
            
            var expected = new WhileInstruction(new BooleanInstruction(true));
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}