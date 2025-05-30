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

    [TestFixture]
    public class FunctionInstructionInterpret
    {
        [Test]
        public void ReturnsNewFunctionVariable()
        {
            var result = Runner.Run("def func(asdf, qwer):\n\treturn asdf + qwer");
            
            var functionVariable = new FunctionVariable(
                parameters: 
                [
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ],
                instructions: [
                    new ReturnInstruction(
                        new OperationInstruction(
                            "+", 
                            new VariableInstruction("asdf"), 
                            new VariableInstruction("qwer")))
                ]
            );

            var expected = new Dictionary<string, Variable>()
            {
                ["func"] = functionVariable
            };
            
            Assert.That(result[0], Is.EquivalentTo(expected));
        }
        
    }
}