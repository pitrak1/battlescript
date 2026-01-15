using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class LambdaInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void NoArguments()
        {
            var input = "lambda: 53";
            var expected = new LambdaInstruction(null, [new ReturnInstruction(new NumericInstruction(53))]);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void OneArgument()
        {
            var input = "lambda asdf: 53";
            var parameters = new ParameterSet([new VariableInstruction("asdf")]);
            var expected = new LambdaInstruction(parameters, [new ReturnInstruction(new NumericInstruction(53))]);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void MultipleArguments()
        {
            var input = "lambda asdf, qwer: 53";
            var parameters = new ParameterSet([new VariableInstruction("asdf"), new VariableInstruction("qwer")]);
            var expected = new LambdaInstruction(parameters, [new ReturnInstruction(new NumericInstruction(53))]);
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}