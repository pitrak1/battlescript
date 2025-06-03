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
            var expected = new NumberVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsSubtraction()
        {
            var input = "x = 6\nx -= 3";
            var expected = new NumberVariable(3);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsMultiplication()
        {
            var input = "x = 6\nx *= 3";
            var expected = new NumberVariable(18);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsDivision()
        {
            var input = "x = 6\nx /= 3";
            var expected = new NumberVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsModulo()
        {
            var input = "x = 6\nx %= 3";
            var expected = new NumberVariable(0);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsFloorDivision()
        {
            var input = "x = 7\nx //= 3";
            var expected = new NumberVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsPower()
        {
            var input = "x = 6\nx **= 2";
            var expected = new NumberVariable(36);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsBitwiseAnd()
        {
            // 0110 & 0011 = 0010
            var input = "x = 6\nx &= 3";
            var expected = new NumberVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsBitwiseOr()
        {
            // 0110 | 0011 = 0111
            var input = "x = 6\nx |= 3";
            var expected = new NumberVariable(7);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsBitwiseXor()
        {
            // 0110 ^ 0011 = 0101
            var input = "x = 6\nx ^= 3";
            var expected = new NumberVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsLeftShift()
        {
            // 0011 << 2 = 1100
            var input = "x = 3\nx <<= 2";
            var expected = new NumberVariable(12);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsRightShift()
        {
            // 0110 >> 2 = 0001
            var input = "x = 6\nx >>= 2";
            var expected = new NumberVariable(1);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}