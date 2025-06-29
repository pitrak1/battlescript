using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ArrayInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void SplitsByCommas()
        {
            var lexer = new Lexer("5, 6");
            var lexerResult = lexer.Run();

            var expected = new ArrayInstruction(",", [
                new IntegerInstruction(5),
                new IntegerInstruction(6),
            ]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void SplitsByColons()
        {
            var lexer = new Lexer("5: 6");
            var lexerResult = lexer.Run();

            var expected = new ArrayInstruction(":", [
                new IntegerInstruction(5),
                new IntegerInstruction(6),
            ]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void PrioritizesCommasOverColons()
        {
            var lexer = new Lexer("5: 6, 7: 8");
            var lexerResult = lexer.Run();

            var expected = new ArrayInstruction(",", [
                new ArrayInstruction(":", [new IntegerInstruction(5), new IntegerInstruction(6)]),
                new ArrayInstruction(":", [new IntegerInstruction(7), new IntegerInstruction(8)]),
            ]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsForBlankValues()
        {
            var lexer = new Lexer("5:2:");
            var lexerResult = lexer.Run();

            var expected = new ArrayInstruction(":", [
                new IntegerInstruction(5), 
                new IntegerInstruction(6),
                null
            ]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }
}