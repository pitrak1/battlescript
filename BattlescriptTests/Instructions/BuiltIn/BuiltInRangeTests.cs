using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInRangeTests
{
    [Test]
    public void SingleArgument()
    {
        var input = "x = range(5)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 0),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
        });
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void TwoArguments()
    {
        var input = "x = range(2, 5)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
        });
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void ThreeArguments()
    {
        var input = "x = range(2, 10, 2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
            BtlTypes.Create(BtlTypes.Types.Int, 6),
            BtlTypes.Create(BtlTypes.Types.Int, 8),
        });
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void CountNotMatchingStep()
    {
        var input = "x = range(2, 5, 2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
        });
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void DecreasingRange()
    {
        var input = "x = range(2, -5, -2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 0),
            BtlTypes.Create(BtlTypes.Types.Int, -2),
            BtlTypes.Create(BtlTypes.Types.Int, -4),
        });
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void ReturnsEmptyListIfGivenInfiniteRange()
    {
        var input = "x = range(2, -5, 2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}