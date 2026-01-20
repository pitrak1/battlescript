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
            var result = Runner.Parse("return 4");
            Assert.That(result[0], Is.EqualTo(expected));
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
            Assert.That(closure.GetVariable(callStack, "func"), Is.EqualTo(funcVariable));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 9)));
        }
    }
}
