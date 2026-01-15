using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class WhileInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ParsesCondition()
        {
            var input = "while True:";
            var expected = new WhileInstruction(new ConstantInstruction("True"));
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}