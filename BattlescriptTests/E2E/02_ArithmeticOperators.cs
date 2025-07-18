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
            var expected = new IntegerVariable(11);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsSubtraction()
        {
            var input = "x = 5 - 3";
            var expected = new IntegerVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsMultiplication()
        {
            var input = "x = 5 * 3";
            var expected = new IntegerVariable(15);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsDivision()
        {
            var input = "x = 5.0 / 2.0";
            var expected = new FloatVariable(2.5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsFloorDivision()
        {
            var input = "x = 5 // 2";
            var expected = new IntegerVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsModulo()
        {
            var input = "x = 5 % 2";
            var expected = new IntegerVariable(1);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsPower()
        {
            var input = "x = 5 ** 2";
            var expected = new IntegerVariable(25);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}