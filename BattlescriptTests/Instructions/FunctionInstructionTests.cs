using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class FunctionInstructionParse
    {
        [Test]
        public void HandlesSimpleDefinition()
        {
            var lexer = new Lexer("def func():");
            var lexerResult = lexer.Run();
            
            var expected = new FunctionInstruction(
                name: "func"
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesDefinitionWithPositionalArgument()
        {
            var lexer = new Lexer("def func(asdf):");
            var lexerResult = lexer.Run();
            
            var expected = new FunctionInstruction(
                name: "func",
                parameters: [new VariableInstruction("asdf")]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesDefinitionWithMultiplePositionalArguments()
        {
            var lexer = new Lexer("def func(asdf, qwer):");
            var lexerResult = lexer.Run();
            
            var expected = new FunctionInstruction(
                name: "func",
                parameters: 
                [
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }
}