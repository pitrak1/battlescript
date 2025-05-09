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
        
        // This is actually not built in the ParensInstruction class yet
        // [Test]
        // public void AllowsCreatingClassObjects()
        // {
        //     var input = "class asdf:\n\ti = 1234\nx = asdf()";
        //     var expected = new ObjectVariable(
        //         new Dictionary<string, Variable>()
        //         {
        //             { "i", new NumberVariable(1234) }
        //         });
        //     E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        // }
        
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
        
        // [Test]
        // public void AllowsAccessingValueMembers()
        // {
        //     var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
        //     var expected = new NumberVariable(1234);
        //     E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        // }
        //
        // [Test]
        // public void ChangingValueMembersDoesNotAlterClass()
        // {
        //     var input = "class asdf:\n\ti = 1234\nx = asdf()\nx.i = 6";
        //     var expected = new ClassVariable(
        //         new Dictionary<string, Variable>()
        //         {
        //             { "i", new NumberVariable(1234) }
        //         }
        //     );
        //     E2EAssertions.AssertVariableValueFromInput(input, "asdf", expected);
        // }
    }
}