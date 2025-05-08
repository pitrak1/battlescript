using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class ComparisonOperators
    {
        [Test]
        public void SupportsEquals()
        {
            var input = "x = 5 == 6\ny = 5 == 5";
            
            var expected1 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
        }
        
        [Test]
        public void SupportsNotEquals()
        {
            var input = "x = 5 != 6\ny = 5 != 5";
            
            var expected1 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
        }
        
        [Test]
        public void SupportsGreaterThan()
        {
            var input = "x = 5 > 6\ny = 6 > 6\nz = 7 > 6";
            
            var expected1 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
            
            var expected3 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "z", expected3);
        }
        
        [Test]
        public void SupportsLessThan()
        {
            var input = "x = 5 < 6\ny = 6 < 6\nz = 7 < 6";
            
            var expected1 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
            
            var expected3 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "z", expected3);
        }
        
        [Test]
        public void SupportsGreaterThanOrEqualTo()
        {
            var input = "x = 5 >= 6\ny = 6 >= 6\nz = 7 >= 6";
            
            var expected1 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
            
            var expected3 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "z", expected3);
        }
        
        [Test]
        public void SupportsLessThanOrEqualTo()
        {
            var input = "x = 5 <= 6\ny = 6 <= 6\nz = 7 <= 6";
            
            var expected1 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected1);
            
            var expected2 = new BooleanVariable(true);
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected2);
            
            var expected3 = new BooleanVariable(false);
            E2EAssertions.AssertVariableValueFromInput(input, "z", expected3);
        }
    }
}