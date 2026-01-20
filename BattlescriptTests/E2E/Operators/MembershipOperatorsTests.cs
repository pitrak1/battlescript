using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class MembershipOperatorsTests
{
    [Test]
    public void InOperationWithString()
    {
        var input = """
                    x = 'sd' in 'asdf'
                    y = 'fa' in 'asdf'
                    """;
        var (callStack, closure) = Runner.Run(input);

        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.True));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.False));
    }
        
    [Test]
    public void NotInOperationWithString()
    {
        var input = """
                    x = 'sd' not in 'asdf'
                    y = 'fa' not in 'asdf'
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.False));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.True));
    }
    
    [Test]
    public void InOperationWithList()
    {
        var input = """
                    x = 'c' in ['a', 'b', 'c', 'd']
                    y = 'f' in ['a', 'b', 'c', 'd']
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.True));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.False));
    }
    
    [Test]
    public void NotInOperationWithList()
    {
        var input = """
                    x = 'c' not in ['a', 'b', 'c', 'd']
                    y = 'f' not in ['a', 'b', 'c', 'd']
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.False));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.True));
    }
    
    [Test]
    public void InOperationWithDictionaryAndStringKeys()
    {
        var input = """
                    x = 'c' in {'a': 'b', 'c': 'd'}
                    y = 'f' in {'a': 'b', 'c': 'd'}
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.True));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.False));
    }
        
    [Test]
    public void NotInOperationWithDictionaryAndStringKeys()
    {
        var input = """
                    x = 'c' not in {'a': 'b', 'c': 'd'}
                    y = 'f' not in {'a': 'b', 'c': 'd'}
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.False));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.True));
    }
    
    [Test]
    public void InOperationWithDictionaryAndIntKeys()
    {
        var input = """
                    x = 2 in {1: 'b', 2: 'd'}
                    y = 4 in {1: 'b', 2: 'd'}
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.True));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.False));
    }
        
    [Test]
    public void NotInOperationWithDictionaryAndIntKeys()
    {
        var input = """
                    x = 2 not in {1: 'b', 2: 'd'}
                    y = 4 not in {1: 'b', 2: 'd'}
                    """;
        var (callStack, closure) = Runner.Run(input);
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.False));
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.True));
    }
}