using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class Lists
    {
        [Test]
        public void SupportsListCreation()
        {
            var input = "x = [5, '5']";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new IntegerVariable(5),
                    new StringVariable("5"),
                }
            );
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }

        [Test]
        public void SupportsListIndexing()
        {
            var input = "x = [5, '5']\ny = x[1]";
            var expected = new StringVariable("5");
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected));
        }

        [Test]
        public void SupportsListRangeIndexing()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:2]";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new IntegerVariable(3),
                    new IntegerVariable(2),
                }
            );
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNullStart()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[:2]";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new IntegerVariable(5),
                    new IntegerVariable(3),
                    new IntegerVariable(2),
                }
            );
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNullEnd()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:]";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new IntegerVariable(3),
                    new IntegerVariable(2),
                    new StringVariable("5"),
                }
            );
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected));
        }
    }
}