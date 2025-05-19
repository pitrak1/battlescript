using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ParensInstructionParse
    {
        [Test]
        public void HandlesSimpleValueLists()
        {
            var lexer = new Lexer("(4, 'asdf')");
            var lexerResult = lexer.Run();
            
            var expected = new ParensInstruction(
                [
                    new NumberInstruction(4),
                    new StringInstruction("asdf")
                ]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}