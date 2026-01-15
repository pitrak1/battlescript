using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ConversionTypeInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void Basic()
        {
            var input = "__btl_numeric__";
            var expected = new ConversionTypeInstruction("__btl_numeric__");
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParenthesesWithoutArguments()
        {
            var input = "__btl_string__()";
            var expected = new ConversionTypeInstruction("__btl_string__");
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void ParenthesesWithArguments()
        {
            var input = "__btl_mapping__(asdf, qwer)";
            var expected = new ConversionTypeInstruction(
                "__btl_mapping__",
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")]
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}