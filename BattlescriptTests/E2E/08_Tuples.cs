using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class Tuples
    {
        [Test]
        public void SupportsTupleDefinition()
        {
            var input = "x = (5, '5')";
            var expected = new Variable(
                Consts.VariableTypes.Tuple,
                new List<Variable>
                {
                    new(Consts.VariableTypes.Number, 5),
                    new(Consts.VariableTypes.String, "5"),
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }

        [Test]
        public void SupportsTupleIndexing()
        {
            var input = "x = (5, '5')\ny = x[1]";
            var expected = new Variable(Consts.VariableTypes.String, "5");
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
    }
}