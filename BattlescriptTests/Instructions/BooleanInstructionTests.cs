using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class BooleanInstructionParse
    {
        [Test]
        public void HandlesSimpleBooleanParsing()
        {
            var lexer = new Lexer("False");
            var lexerResult = lexer.Run();
            
            var expected = new BooleanInstruction(false);
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class BooleanInstructionInterpret
    {
        [Test]
        public void ReturnsNewBooleanVariable()
        {
            var result = Runner.Run("x = False");
            var expected = new Dictionary<string, Variable>
            {
                { "x", new BooleanVariable(false) }
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
    }
}