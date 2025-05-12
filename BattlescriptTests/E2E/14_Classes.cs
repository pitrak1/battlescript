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
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", new NumberVariable(1234)}
                });
            E2EAssertions.AssertVariableValueFromInput(input, "asdf", expected);
        }
        
        [Test]
        public void AllowsCreatingClassObjects()
        {
            var values = new Dictionary<string, Variable>()
            {
                { "i", new NumberVariable(1234) }
            };
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var expected = new ObjectVariable(
                values,
                new ClassVariable(values));
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void IgnoresMethodsWhenCreatingObjects()
        {
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", new NumberVariable(1234) },
                { "j", new FunctionVariable([], [])}
            };
            var objectValues = new Dictionary<string, Variable>()
            {
                { "i", new NumberVariable(1234) }
            };
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var expected = new ObjectVariable(
                objectValues,
                new ClassVariable(classValues));
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void AllowsSuperclasses()
        {
            var input = "class asdf:\n\ti = 1234\nclass qwer(asdf):\n\tj = 2345";
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "j", new NumberVariable(2345) }
                });
            E2EAssertions.AssertVariableValueFromInput(input, "qwer", expected);
        }
        
        [Test]
        public void AllowsAccessingValueMembers()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
            var expected = new NumberVariable(1234);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
        
        [Test]
        public void ChangingValueMembersDoesNotAlterClass()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\nx.i = 6";
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "i", new NumberVariable(1234) }
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "asdf", expected);
        }
        
        [Test]
        public void AllowsAccessingMethods()
        {
            var input = @"
class asdf:
    i = 1234
    def j():
        i = 2345
x = asdf()
x.j()
y = x.i
";
            var expected = new NumberVariable(2345);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
    }
}