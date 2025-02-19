using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class BasicVariableTypes
    {
        [Test]
        public void SupportsStringVariablesUsingSingleQuotes()
        {
            var input = "x = 'asdf'";
            var expected = new Variable(Consts.VariableTypes.String, "asdf");
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void SupportsStringVariablesUsingDoubleQuotes()
        {
            var input = "x = \"asdf\"";
            var expected = new Variable(Consts.VariableTypes.String, "asdf");
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        // We ultimately need to separate this into ints and floats
        [Test]
        public void SupportsNumbers()
        {
            var input = "x = 5.5";
            var expected = new Variable(Consts.VariableTypes.Number, 5.5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }

        [Test]
        public void SupportsBooleans()
        {
            var input = "x = True";
            var expected = new Variable(Consts.VariableTypes.Boolean, true);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}