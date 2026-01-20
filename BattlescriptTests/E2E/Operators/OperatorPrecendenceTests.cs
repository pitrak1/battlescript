using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class OperatorPrecedenceTests
{
    [Test]
    public void PrioritizesParenthesesOverPower()
    {
        var input = "x = 3 ** (1 + 2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(27));

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void PrioritizesPowerOverUnaryNegative()
    {
        var input = "x = -2 ** 3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(-8));

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void PrioritizesUnaryNegativeOverMultiplication()
    {
        var input = "x = -2 * -3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(6));

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void PrioritizesMultiplicationOverAddition()
    {
        var input = "x = 4 + 2 * 2";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(8));

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void PrioritizesAdditionOverComparison()
    {
        var input = "x = 4 + 4 == 8";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1));

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void PrioritizesComparisonOverBoolean()
    {
        var input = "x = True == False and True == True";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0));

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void PrioritizesLeftmostOperations()
    {
        var input = "x = 10 - 5 - 2";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(3));
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
}