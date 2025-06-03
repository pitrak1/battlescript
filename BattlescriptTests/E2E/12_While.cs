using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class While
    {
        [Test]
        public void HandlesTrueWhileStatement()
        {
            var input = "x = 5\nwhile x < 10:\n\tx += 1";
            var expected = new NumberVariable(10);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFalseWhileStatement()
        {
            var input = "x = 5\nwhile x == 6:\n\tx = 10";
            var expected = new NumberVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}