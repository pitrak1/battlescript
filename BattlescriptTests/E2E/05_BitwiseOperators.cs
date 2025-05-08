using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class BitwiseOperators
    {
        [Test]
        public void SupportsBitwiseAnd()
        {
            // 0110 & 0011 = 0010
            var input = "x = 6 & 3";
            var expected = new NumberVariable(2);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsBitwiseOr()
        {
            // 0110 | 0011 = 0111
            var input = "x = 6 | 3";
            var expected = new NumberVariable(7);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsBitwiseXor()
        {
            // 0110 ^ 0011 = 0101
            var input = "x = 6 ^ 3";
            var expected = new NumberVariable(5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsBitwiseNot()
        {
            var input = "x = ~5";
            var expected = new NumberVariable(-6);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsLeftShift()
        {
            // 0011 << 2 = 1100
            var input = "x = 3 << 2";
            var expected = new NumberVariable(12);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsRightShift()
        {
            // 0110 >> 2 = 0001
            var input = "x = 6 >> 2";
            var expected = new NumberVariable(1);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}