using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class BuiltInInstructionParse
    {
        [Test]
        public void HandlesNoArguments()
        {
            var lexer = new Lexer("super()");
            var lexerResult = lexer.Run();
            
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: []
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesArguments()
        {
            var lexer = new Lexer("super(x, y)");
            var lexerResult = lexer.Run();
            
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: [new VariableInstruction("x"), new VariableInstruction("y")]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesTokensAfterArguments()
        {
            var lexer = new Lexer("super(x, y).asdf");
            var lexerResult = lexer.Run();
            
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: [new VariableInstruction("x"), new VariableInstruction("y")],
                next: new SquareBracketsInstruction([new StringInstruction("asdf")])
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class BuiltInInstructionInterpret
    {
        
    }
}