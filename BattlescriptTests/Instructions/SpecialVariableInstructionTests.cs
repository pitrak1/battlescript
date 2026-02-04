using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class VariableInstructionWithAsterisksTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void SingleAsterisk()
        {
            var tokens = new List<Token> { new(Consts.TokenTypes.SpecialVariable, "*args") };
            var result = new VariableInstruction(tokens);

            Assert.That(result.Name, Is.EqualTo("args"));
            Assert.That(result.Asterisks, Is.EqualTo(1));
        }

        [Test]
        public void DoubleAsterisk()
        {
            var tokens = new List<Token> { new(Consts.TokenTypes.SpecialVariable, "**kwargs") };
            var result = new VariableInstruction(tokens);

            Assert.That(result.Name, Is.EqualTo("kwargs"));
            Assert.That(result.Asterisks, Is.EqualTo(2));
        }

        [Test]
        public void CustomSingleAsteriskName()
        {
            var tokens = new List<Token> { new(Consts.TokenTypes.SpecialVariable, "*items") };
            var result = new VariableInstruction(tokens);

            Assert.That(result.Name, Is.EqualTo("items"));
            Assert.That(result.Asterisks, Is.EqualTo(1));
        }

        [Test]
        public void CustomDoubleAsteriskName()
        {
            var tokens = new List<Token> { new(Consts.TokenTypes.SpecialVariable, "**options") };
            var result = new VariableInstruction(tokens);

            Assert.That(result.Name, Is.EqualTo("options"));
            Assert.That(result.Asterisks, Is.EqualTo(2));
        }
    }
}
