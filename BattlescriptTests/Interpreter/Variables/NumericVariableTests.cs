using Battlescript;

namespace BattlescriptTests.InterpreterTests.Variables;

[TestFixture]
public static class NumericVariableTests
{
    [TestFixture]
    public class Operate
    {
        private Memory _memory;
        
        [SetUp]
        public void Setup()
        {
            _memory = Runner.Run("");
        }
        
        [Test]
        public void HandlesNumericOperators()
        {
            var num = new NumericVariable(5);
            var result = num.Operate(_memory, "+", new NumericVariable(10));
            Assertions.AssertVariablesEqual(result, new NumericVariable(15));
        }
        
        [Test]
        public void HandlesComparisonOperators()
        {
            var num = new NumericVariable(5);
            var result = num.Operate(_memory, ">=", new NumericVariable(2));
            Assertions.AssertVariablesEqual(result, new NumericVariable(1));
        }
        
        [Test]
        public void HandlesUnaryNumericOperators()
        {
            var num = new NumericVariable(5);
            var result = num.Operate(_memory, "-", null);
            Assertions.AssertVariablesEqual(result, new NumericVariable(-5));
        }
        
        [Test]
        public void HandlesInversionOnNonCommutativeOperators()
        {
            var num = new NumericVariable(5);
            var result = num.Operate(_memory, "-", new NumericVariable(10), true);
            Assertions.AssertVariablesEqual(result, new NumericVariable(5));
        }
        
        [Test]
        public void HandlesInversionOnCommutativeOperators()
        {
            var num = new NumericVariable(5);
            var result = num.Operate(_memory, "*", new NumericVariable(10), true);
            Assertions.AssertVariablesEqual(result, new NumericVariable(50));
        }
    }
}