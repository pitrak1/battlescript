using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class RangeTests
{
    [Test]
    public void SingleArgumentGeneratesFromZero()
    {
        var (callStack, closure) = Runner.Run("x = range(5)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 0),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void SingleArgumentZeroReturnsEmptyList()
    {
        var (callStack, closure) = Runner.Run("x = range(0)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void SingleArgumentOneReturnsSingleElement()
    {
        var (callStack, closure) = Runner.Run("x = range(1)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 0),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void TwoArgumentsStartAndStop()
    {
        var (callStack, closure) = Runner.Run("x = range(2, 5)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void TwoArgumentsEqualStartAndStopReturnsEmpty()
    {
        var (callStack, closure) = Runner.Run("x = range(3, 3)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void TwoArgumentsStartGreaterThanStopReturnsEmpty()
    {
        var (callStack, closure) = Runner.Run("x = range(5, 2)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void TwoArgumentsNegativeStartToPositiveStop()
    {
        var (callStack, closure) = Runner.Run("x = range(-2, 3)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, -2),
            BtlTypes.Create(BtlTypes.Types.Int, -1),
            BtlTypes.Create(BtlTypes.Types.Int, 0),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ThreeArgumentsWithStep()
    {
        var (callStack, closure) = Runner.Run("x = range(0, 10, 3)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 0),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 6),
            BtlTypes.Create(BtlTypes.Types.Int, 9),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ThreeArgumentsStepDoesNotDivideEvenly()
    {
        var (callStack, closure) = Runner.Run("x = range(2, 5, 2)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void NegativeStepDecreasing()
    {
        var (callStack, closure) = Runner.Run("x = range(5, 0, -1)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 5),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void NegativeStepLargerThanOne()
    {
        var (callStack, closure) = Runner.Run("x = range(10, 0, -3)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 10),
            BtlTypes.Create(BtlTypes.Types.Int, 7),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void NegativeStepWithNegativeRange()
    {
        var (callStack, closure) = Runner.Run("x = range(-1, -5, -1)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, -1),
            BtlTypes.Create(BtlTypes.Types.Int, -2),
            BtlTypes.Create(BtlTypes.Types.Int, -3),
            BtlTypes.Create(BtlTypes.Types.Int, -4),
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void PositiveStepWithDecreasingRangeReturnsEmpty()
    {
        var (callStack, closure) = Runner.Run("x = range(5, 2, 1)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void NegativeStepWithIncreasingRangeReturnsEmpty()
    {
        var (callStack, closure) = Runner.Run("x = range(2, 5, -1)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void UsedInForLoop()
    {
        var input = """
                    total = 0
                    for i in range(5):
                        total += i
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "total"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 10)));
    }

    [Test]
    public void UsedInForLoopWithStartAndStop()
    {
        var input = """
                    total = 0
                    for i in range(3, 7):
                        total += i
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "total"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 18)));
    }

    [Test]
    public void UsedInForLoopWithStep()
    {
        var input = """
                    total = 0
                    for i in range(0, 10, 2):
                        total += i
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "total"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 20)));
    }

    [Test]
    public void SingleArgumentNegativeReturnsEmpty()
    {
        var (callStack, closure) = Runner.Run("x = range(-3)");
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ResultIsAList()
    {
        var input = """
                    x = range(3)
                    x[0] = 99
                    y = x[0]
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
    }
}
