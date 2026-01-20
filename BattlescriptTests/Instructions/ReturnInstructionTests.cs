using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ReturnInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ReturnValue()
        {
            var expected = new ReturnInstruction(new NumericInstruction(4));
            Assertions.AssertInputProducesParserOutput("return 4", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void Expressions()
        {
            var (callStack, closure) = Runner.Run("def func(asdf):\n\treturn asdf + 5\nx = func(4)");

            var funcVariable = new FunctionVariable(
                "func",
                closure,
                new ParameterSet([new VariableInstruction("asdf")]),
                [
                    new ReturnInstruction(
                        new OperationInstruction(
                            "+",
                            new VariableInstruction("asdf"),
                            new NumericInstruction(5)))
                ]);
            Assertions.AssertVariable(callStack, closure, "func", funcVariable);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, 9));
        }
    }
}