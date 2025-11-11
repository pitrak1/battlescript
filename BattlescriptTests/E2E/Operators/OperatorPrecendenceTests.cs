using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class OperatorPrecedenceTests
{
    [Test]
    public void PrioritizesParenthesesOverPower()
    {
        var input = "x = 3 ** (1 + 2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(27));

        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void PrioritizesPowerOverUnaryNegative()
    {
        var input = "x = -2 ** 3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(-8));

        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void PrioritizesUnaryNegativeOverMultiplication()
    {
        var input = "x = -2 * -3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(6));

        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void PrioritizesMultiplicationOverAddition()
    {
        var input = "x = 4 + 2 * 2";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(8));

        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void PrioritizesAdditionOverComparison()
    {
        var input = "x = 4 + 4 == 8";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1));

        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void PrioritizesComparisonOverBoolean()
    {
        var input = "x = True == False and True == True";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0));

        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void PrioritizesLeftmostOperations()
    {
        var input = "x = 10 - 5 - 2";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, new NumericVariable(3));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}