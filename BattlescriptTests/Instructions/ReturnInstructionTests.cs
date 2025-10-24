using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ReturnInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleValue()
        {
            var expected = new ReturnInstruction(new NumericInstruction(4));
            Assertions.AssertInputProducesParserOutput("return 4", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesExpressions()
        {
            var memory = Runner.Run("def func(asdf):\n\treturn asdf + 5\nx = func(4)");

            var funcVariable = new FunctionVariable(
                "func",
                new ParameterSet([new VariableInstruction("asdf")]),
                [
                    new ReturnInstruction(
                        new OperationInstruction(
                            "+",
                            new VariableInstruction("asdf"),
                            new NumericInstruction(5)))
                ]);
            Assertions.AssertVariable(memory, "func", funcVariable);
            Assertions.AssertVariable(memory, "x", BsTypes.Create(BsTypes.Types.Int, 9));
        }
    }
}