using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class IfInstructionParse
    {
        [Test]
        public void ProperlyParsesCondition()
        {
            var lexer = new Lexer("if True:");
            var lexerResult = lexer.Run();
            
            var expected = new IfInstruction(new BooleanInstruction(true));
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}