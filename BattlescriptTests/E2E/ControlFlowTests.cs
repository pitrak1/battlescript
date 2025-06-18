using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ControlFlowTests
{
    [TestFixture]
    public class IfElifElse
    {
        [Test]
        public void HandlesTrueIfStatement()
        {
            var input = "x = 5\nif x == 5:\n\tx = 6";
            var expected = new IntegerVariable(6);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFalseIfStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsElseStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelse:\n\tx = 7";
            var expected = new IntegerVariable(7);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }

        [Test]
        public void SupportsElifStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelif x < 8:\n\tx = 9\nelse:\n\tx = 7";
            var expected = new IntegerVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsElifStatementWithoutElse()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelif x < 8:\n\tx = 9";
            var expected = new IntegerVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsConsecutiveIfStatements()
        {
            var input = "x = 5\nif x < 6:\n\tx = 7\nif x >= 7:\n\tx = 9";
            var expected = new IntegerVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class While
    {
        [Test]
        public void HandlesTrueWhileStatement()
        {
            var input = "x = 5\nwhile x < 10:\n\tx += 1";
            var expected = new IntegerVariable(10);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFalseWhileStatement()
        {
            var input = "x = 5\nwhile x == 6:\n\tx = 10";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}