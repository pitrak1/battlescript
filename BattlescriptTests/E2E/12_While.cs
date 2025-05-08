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
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void HandlesFalseWhileStatement()
        {
            var input = "x = 5\nwhile x == 6:\n\tx = 10";
            var expected = new NumberVariable(5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}