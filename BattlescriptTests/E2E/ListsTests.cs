using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class ListsTests
{
    [TestFixture]
    public class Methods
    {
        [Test]
        public void Append()
        {
            var input = """
                        x = []
                        x.append(1)
                        """;
            var memory = Runner.Run(input);
            var expected = new ListVariable([BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1)]);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
}