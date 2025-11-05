using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class FunctionInstructionTests
{
    [TestFixture]
    public class Parse
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
        public void HandlesDefinitionWithArgument()
        {
            var expected = new FunctionInstruction(
                name: "func",
                parameters: new ParameterSet([new VariableInstruction("asdf")])
            );
            Assertions.AssertInputProducesParserOutput("def func(asdf):", expected);
        }
        
        [Test]
        public void HandlesDefinitionWithMultipleArguments()
        {
            var expected = new FunctionInstruction(
                name: "func",
                parameters: 
                new ParameterSet([
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ])
            );
            Assertions.AssertInputProducesParserOutput("def func(asdf, qwer):", expected);
        }
        
        [Test]
        public void HandlesDefinitionWithDefaultArguments()
        {
            var expected = new FunctionInstruction(
                name: "func",
                parameters: 
                new ParameterSet([
                    new VariableInstruction("asdf"),
                    new AssignmentInstruction("=", new VariableInstruction("qwer"), new NumericInstruction(1234))
                ])
            );
            Assertions.AssertInputProducesParserOutput("def func(asdf, qwer=1234):", expected);
        }
        
        [Test]
        public void ThrowsErrorIfDefaultArgumentIsBeforeRequiredArgument()
        {
            Assert.Throws<InterpreterRequiredParamFollowsDefaultParamException>(() => Assertions.AssertInputProducesParserOutput("def func(qwer=1234, asdf):", new NumericInstruction(1234)));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void ReturnsNewFunctionVariable()
        {
            var (callStack, closure) = Runner.Run("def func(asdf, qwer):\n\treturn asdf + qwer");
            
            var functionVariable = new FunctionVariable(
                "func",
                closure,
                parameters: 
                new ParameterSet([
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ]),
                instructions: [
                    new ReturnInstruction(
                        new OperationInstruction(
                            "+", 
                            new VariableInstruction("asdf"), 
                            new VariableInstruction("qwer")))
                ]
            );
            Assertions.AssertVariable(callStack, closure, "func", functionVariable);
        }
        
    }
}