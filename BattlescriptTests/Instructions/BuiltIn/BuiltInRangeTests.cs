using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInRangeTests
{
    [Test]
    public void HandlesSingleArgument()
    {
        var input = "x = range(5)";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new List<Variable>() {
            memory.Create(Memory.BsTypes.Int, 0),
            memory.Create(Memory.BsTypes.Int, 1),
            memory.Create(Memory.BsTypes.Int, 2),
            memory.Create(Memory.BsTypes.Int, 3),
            memory.Create(Memory.BsTypes.Int, 4),
        });
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesTwoArguments()
    {
        var input = "x = range(2, 5)";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new List<Variable>() {
            memory.Create(Memory.BsTypes.Int, 2),
            memory.Create(Memory.BsTypes.Int, 3),
            memory.Create(Memory.BsTypes.Int, 4),
        });
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesThreeArguments()
    {
        var input = "x = range(2, 10, 2)";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new List<Variable>() {
            memory.Create(Memory.BsTypes.Int, 2),
            memory.Create(Memory.BsTypes.Int, 4),
            memory.Create(Memory.BsTypes.Int, 6),
            memory.Create(Memory.BsTypes.Int, 8),
        });
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesCountNotMatchingStep()
    {
        var input = "x = range(2, 5, 2)";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new List<Variable>() {
            memory.Create(Memory.BsTypes.Int, 2),
            memory.Create(Memory.BsTypes.Int, 4),
        });
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesDecreasingRange()
    {
        var input = "x = range(2, -5, -2)";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new List<Variable>() {
            memory.Create(Memory.BsTypes.Int, 2),
            memory.Create(Memory.BsTypes.Int, 0),
            memory.Create(Memory.BsTypes.Int, -2),
            memory.Create(Memory.BsTypes.Int, -4),
        });
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void ReturnsEmptyListIfGivenInfiniteRange()
    {
        var input = "x = range(2, -5, 2)";
        var memory = Runner.Run(input);
        var expected = memory.Create(Memory.BsTypes.List, new List<Variable>());
        
        Assertions.AssertVariable(memory, "x", expected);
    }
}