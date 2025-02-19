using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void SupportsAddition()
        {
            var input = "x = 6\nx += 3";
            var expected = new Variable(Consts.VariableTypes.Number, 9);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsSubtraction()
        {
            var input = "x = 6\nx -= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 3);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsMultiplication()
        {
            var input = "x = 6\nx *= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 18);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsDivision()
        {
            var input = "x = 6\nx /= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 2);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsModulo()
        {
            var input = "x = 6\nx %= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 0);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsFloorDivision()
        {
            var input = "x = 7\nx //= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 2);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsPower()
        {
            var input = "x = 6\nx **= 2";
            var expected = new Variable(Consts.VariableTypes.Number, 36);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsBitwiseAnd()
        {
            // 0110 & 0011 = 0010
            var input = "x = 6\nx &= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 2);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsBitwiseOr()
        {
            // 0110 | 0011 = 0111
            var input = "x = 6\nx |= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 7);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsBitwiseXor()
        {
            // 0110 ^ 0011 = 0101
            var input = "x = 6\nx ^= 3";
            var expected = new Variable(Consts.VariableTypes.Number, 5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsLeftShift()
        {
            // 0011 << 2 = 1100
            var input = "x = 3\nx <<= 2";
            var expected = new Variable(Consts.VariableTypes.Number, 12);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsRightShift()
        {
            // 0110 >> 2 = 0001
            var input = "x = 6\nx >>= 2";
            var expected = new Variable(Consts.VariableTypes.Number, 1);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}