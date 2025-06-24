using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ListsTests
{
    [TestFixture]
    public class Methods
    {
        [Test]
        public void Append()
        {
            var input = @"
x = []
x.append(1)";
            var expected = new ListVariable([new IntegerVariable(1)]);
            var memory = Runner.Run(input);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
}