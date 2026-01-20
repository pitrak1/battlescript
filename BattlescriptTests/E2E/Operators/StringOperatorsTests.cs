using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class StringOperatorsTests
{
    [Test]
    public void Addition()
    {
        var input = "x = 'asdf' + 'qwer'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfqwer"));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void AdditionAssignment()
    {
        var input = "x = 'asdf'\nx += 'qwer'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfqwer"));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void Multiplication()
    {
        var input = """
                    x = 'asdf' * 3
                    y = 3 * 'asdf'
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdfasdf"));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
    
    [Test]
    public void MultiplicationAssignment()
    {
        var input = "x = 'asdf'\nx *= 3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdfasdf"));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void Equals()
    {
        var input = """
                    x = 'asdf' == 'asdf'
                    y = 'asdf' == 'qwer'
                    """;
        var (callStack, closure) = Runner.Run(input);

        Assertions.AssertVariable(callStack, closure, "x", BtlTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BtlTypes.False);
    }
    
    [Test]
    public void NotEquals()
    {
        var input = """
                    x = 'asdf' != 'asdf'
                    y = 'asdf' != 'qwer'
                    """;
        var (callStack, closure) = Runner.Run(input);

        Assertions.AssertVariable(callStack, closure, "x", BtlTypes.False);
        Assertions.AssertVariable(callStack, closure, "y", BtlTypes.True);
    }
}