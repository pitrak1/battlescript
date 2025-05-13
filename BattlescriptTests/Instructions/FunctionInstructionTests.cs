using Battlescript;

namespace BattlescriptTests;

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
            ParserAssertions.AssertInputProducesInstruction("def func():", expected);
        }
        
        [Test]
        public void HandlesDefinitionWithPositionalArgument()
        {
            var expected = new FunctionInstruction(
                name: "func",
                parameters: [new VariableInstruction("asdf")]
            );
            ParserAssertions.AssertInputProducesInstruction("def func(asdf):", expected);
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
            ParserAssertions.AssertInputProducesInstruction("def func(asdf, qwer):", expected);
        }
    }
}