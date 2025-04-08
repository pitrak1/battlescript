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
    }
}