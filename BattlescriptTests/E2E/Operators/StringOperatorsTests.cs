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
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void AdditionAssignment()
    {
        var input = "x = 'asdf'\nx += 'qwer'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfqwer"));
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
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
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
    }
    
    [Test]
    public void MultiplicationAssignment()
    {
        var input = "x = 'asdf'\nx *= 3";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdfasdf"));
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void Equals()
    {
        var input = """
                    x = 'asdf' == 'asdf'
                    y = 'asdf' == 'qwer'
                    """;
        var (callStack, closure) = Runner.Run(input);

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.True));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.False));
    }
    
    [Test]
    public void NotEquals()
    {
        var input = """
                    x = 'asdf' != 'asdf'
                    y = 'asdf' != 'qwer'
                    """;
        var (callStack, closure) = Runner.Run(input);

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.False));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.True));
    }
}