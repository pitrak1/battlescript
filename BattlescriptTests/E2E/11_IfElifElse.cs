using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class IfElifElse
    {
        [Test]
        public void HandlesTrueIfStatement()
        {
            var input = "x = 5\nif x == 5:\n\tx = 6";
            var expected = new Variable(Consts.VariableTypes.Number, 6);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void HandlesFalseIfStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6";
            var expected = new Variable(Consts.VariableTypes.Number, 5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}