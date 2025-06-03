using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class LogicalOperators
    {
        [Test]
        public void SupportsAnd()
        {
            var input = "x = True and False\ny = True and True";
            
            var expected1 = new BooleanVariable(false);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected1));
            
            var expected2 = new BooleanVariable(true);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected2));
        }
        
        [Test]
        public void SupportsOr()
        {
            var input = "x = True or False\ny = False or False";
            
            var expected1 = new BooleanVariable(true);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected1));
            
            var expected2 = new BooleanVariable(false);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected2));
        }
        
        [Test]
        public void SupportsNot()
        {
            var input = "x = not True\ny = not False";
            
            var expected1 = new BooleanVariable(false);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected1));
            
            var expected2 = new BooleanVariable(true);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected2));
        }
    }
}