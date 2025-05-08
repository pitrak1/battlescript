using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class ArithmeticOperators
    {
        [Test]
        public void SupportsAddition()
        {
            var input = "x = 5 + 6";
            var expected = new NumberVariable(11);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsSubtraction()
        {
            var input = "x = 5 - 3";
            var expected = new NumberVariable(2);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsMultiplication()
        {
            var input = "x = 5 * 3";
            var expected = new NumberVariable(15);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsDivision()
        {
            var input = "x = 5 / 2";
            var expected = new NumberVariable(2.5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsFloorDivision()
        {
            var input = "x = 5 // 2";
            var expected = new NumberVariable(2);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsModulo()
        {
            var input = "x = 5 % 2";
            var expected = new NumberVariable(1);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsPower()
        {
            var input = "x = 5 ** 2";
            var expected = new NumberVariable(25);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}