namespace BattlescriptTests;

using Battlescript;

[TestFixture]
public static partial class E2ETests
{
    [TestFixture]
    public class OperatorOverloading
    {
        [Test]
        public void AllowsOperatorOverloading()
        {
            var input = @"
class asdf:
    i = 5

    def __add__(self, other):
        return self.i + other.i

x = asdf()
y = asdf()
z = x + y
";
            var expected = new NumberVariable(10.0);
            E2EAssertions.AssertVariableValueFromInput(input, "z", expected);
        }
    }
}