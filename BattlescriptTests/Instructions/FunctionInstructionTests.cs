using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class FunctionInstructionParse
    {
        [Test]
        public void HandlesSimpleDefinition()
        {
            var expected = new FunctionInstruction(
                name: "func"
            );
            Assertions.AssertInputProducesParserOutput("def func():", expected);
        }
        
        [Test]
        public void HandlesDefinitionWithPositionalArgument()
        {
            var expected = new FunctionInstruction(
                name: "func",
                parameters: [new VariableInstruction("asdf")]
            );
            Assertions.AssertInputProducesParserOutput("def func(asdf):", expected);
        }
        
        [Test]
        public void HandlesDefinitionWithMultiplePositionalArguments()
        {
            var expected = new FunctionInstruction(
                name: "func",
                parameters: 
                [
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ]
            );
            Assertions.AssertInputProducesParserOutput("def func(asdf, qwer):", expected);
        }
    }

    [TestFixture]
    public class FunctionInstructionInterpret
    {
        [Test]
        public void ReturnsNewFunctionVariable()
        {
            var memory = Runner.Run("def func(asdf, qwer):\n\treturn asdf + qwer");
            
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
            Assertions.AssertVariablesEqual(memory.Scopes[0]["func"], functionVariable);
        }
        
    }
}