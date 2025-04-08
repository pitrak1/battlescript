using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests
{
    [TestFixture]
    public class Classes
    {
        [Test]
        public void AllowsDefiningClasses()
        {
            var input = "class asdf:\n\ti = 1234";
            var expected = new Variable(
                Consts.VariableTypes.Class,
                new Dictionary<string, Variable>()
                {
                    {"i", new Variable(Consts.VariableTypes.Number, 1234)}
                });
            E2EAssertions.AssertVariableValueFromInput(input, "asdf", expected);
        }
        
        [Test]
        public void AllowsCreatingClassObjects()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var expected = new Variable(
                Consts.VariableTypes.Object,
                new Dictionary<string, Variable>()
                {
                    {"i", new Variable(Consts.VariableTypes.Number, 1234)}
                });
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void AllowsAccessingValueMembers()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
            var expected = new Variable(
                Consts.VariableTypes.Number,
                1234);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
    }
}