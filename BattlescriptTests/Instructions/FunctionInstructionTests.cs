using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class FunctionInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void OneArgument()
        {
            var input = "def func(asdf):";
            var expected = new FunctionInstruction(
                name: "func",
                parameters: new ParameterSet([new VariableInstruction("asdf")])
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void MultipleArguments()
        {
            var input = "def func(asdf, qwer):";
            var expected = new FunctionInstruction(
                name: "func",
                parameters:
                new ParameterSet([
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ])
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void DefaultArguments()
        {
            var input = "def func(asdf, qwer=1234):";
            var expected = new FunctionInstruction(
                name: "func",
                parameters:
                new ParameterSet([
                    new VariableInstruction("asdf"),
                    new AssignmentInstruction("=", new VariableInstruction("qwer"), new NumericInstruction(1234))
                ])
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ThrowsErrorIfDefaultArgumentIsBeforeRequiredArgument()
        {
            Assert.Throws<InternalRaiseException>(() => Runner.Parse("def func(qwer=1234, asdf):"));
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
            Assert.That(closure.GetVariable(callStack, "func"), Is.EqualTo(functionVariable));
        }

    }
}
