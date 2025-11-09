using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class OperatorsTests
{
    [TestFixture]
    public class BooleanOperators
    {
        [Test]
        public void HandlesAndOperations()
        {
            var input = """
                        a = True and True
                        b = True and False
                        c = False and True
                        d = False and False
                        """;
            var (callStack, closure) = Runner.Run(input);
            var trueValue = BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1));
            var falseValue = BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0));
            
            Assertions.AssertVariable(callStack, closure, "a", trueValue);
            Assertions.AssertVariable(callStack, closure, "b", falseValue);
            Assertions.AssertVariable(callStack, closure, "c", falseValue);
            Assertions.AssertVariable(callStack, closure, "d", falseValue);
        }
    }
}