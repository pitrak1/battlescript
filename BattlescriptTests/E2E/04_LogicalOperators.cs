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
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
        }
        
        [Test]
        public void SupportsOr()
        {
            var input = "x = True or False\ny = False or False";
            
            var expected1 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
        }
        
        [Test]
        public void SupportsNot()
        {
            var input = "x = not True\ny = not False";
            
            var expected1 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
        }
    }
}