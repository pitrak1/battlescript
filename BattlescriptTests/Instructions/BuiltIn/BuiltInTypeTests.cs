using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInTypeTests
{
    [Test]
    public void Functions()
    {
        var input = """
                    def y():
                        pass
                        
                    x = type(y)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "<class 'function'>");
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void Classes()
    {
        var input = """
                    class y():
                        pass
                        
                    x = type(y)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "<class 'type'>");
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void TransitionTypeVariables()
    {
        var input = """
                    y = __btl_string__()
                    x = type(y)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "<class '__btl_string__'>");
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void BuiltInVariables()
    {
        var input = """
                    y = 5
                    x = type(y)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "<class 'int'>");
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void ObjectVariables()
    {
        var input = """
                    class y:
                        pass
                    
                    z = y()
                    x = type(z)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "<class 'y'>");
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}