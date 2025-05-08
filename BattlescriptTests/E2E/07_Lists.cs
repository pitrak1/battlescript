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
                    new NumberVariable(5),
                    new StringVariable("5"),
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }

        [Test]
        public void SupportsListIndexing()
        {
            var input = "x = [5, '5']\ny = x[1]";
            var expected = new StringVariable("5");
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }

        [Test]
        public void SupportsListRangeIndexing()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:3]";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new NumberVariable(3),
                    new NumberVariable(2),
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNullStart()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[:3]";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new NumberVariable(5),
                    new NumberVariable(3),
                    new NumberVariable(2),
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNullEnd()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:]";
            var expected = new ListVariable(
                new List<Variable>
                {
                    new NumberVariable(3),
                    new NumberVariable(2),
                    new StringVariable("5"),
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
    }
}