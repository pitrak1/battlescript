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
            var expected = new IntegerVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsSubtraction()
        {
            var input = "x = 6\nx -= 3";
            var expected = new IntegerVariable(3);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsMultiplication()
        {
            var input = "x = 6\nx *= 3";
            var expected = new IntegerVariable(18);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsDivision()
        {
            var input = "x = 6\nx /= 3";
            var expected = new IntegerVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsModulo()
        {
            var input = "x = 6\nx %= 3";
            var expected = new IntegerVariable(0);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsFloorDivision()
        {
            var input = "x = 7\nx //= 3";
            var expected = new IntegerVariable(2);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsPower()
        {
            var input = "x = 6\nx **= 2";
            var expected = new IntegerVariable(36);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}