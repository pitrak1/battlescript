using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class ListOperatorsTests
{
    [Test]
    public void Addition()
    {
        var input = "x = [1, 2, 3] + [4, 5, 6]";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
            BtlTypes.Create(BtlTypes.Types.Int, 5),
            BtlTypes.Create(BtlTypes.Types.Int, 6)
        ]));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void AdditionAssignment()
    {
        var input = "x = [1, 2, 3]\nx += [4, 5, 6]";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.Int, 4),
            BtlTypes.Create(BtlTypes.Types.Int, 5),
            BtlTypes.Create(BtlTypes.Types.Int, 6)
        ]));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void Multiplication()
    {
        var input = """
                    x = [1, 2] * 3
                    y = 3 * [1, 2]
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2)
        ]));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
    
    [Test]
    public void MultiplicationAssignment()
    {
        var input = "x = [1, 2]\nx *= 3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2)
        ]));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void Equals()
    {
        var input = """
                    x = [1, 2, 3] == [1, 2, 3]
                    y = [1, 2, 3] == [1, 2, 4]
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assertions.AssertVariable(callStack, closure, "x", BtlTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BtlTypes.False);
    }
    
    [Test]
    public void NotEquals()
    {
        var input = """
                    x = [1, 2, 3] != [1, 2, 3]
                    y = [1, 2, 3] != [1, 2, 4]
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assertions.AssertVariable(callStack, closure, "x", BtlTypes.False);
        Assertions.AssertVariable(callStack, closure, "y", BtlTypes.True);
    }
}