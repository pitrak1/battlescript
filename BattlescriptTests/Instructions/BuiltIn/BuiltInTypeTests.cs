using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInTypeTests
{
    [Test]
    public void HandlesFunctions()
    {
        var input = """
                    def y():
                        pass
                        
                    x = type(y)
                    """;
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.String, "<class 'function'>");
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesClasses()
    {
        var input = """
                    class y():
                        pass
                        
                    x = type(y)
                    """;
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.String, "<class 'type'>");
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesInternalVariables()
    {
        var input = """
                    y = __string__()
                    x = type(y)
                    """;
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.String, "<class '__string__'>");
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void HandlesBuiltInVariables()
    {
        var input = """
                    y = 5
                    x = type(y)
                    """;
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.String, "<class 'int'>");
        Assertions.AssertVariable(memory, "x", expected);
    }

    [Test]
    public void HandlesObjectVariables()
    {
        var input = """
                    class y:
                        pass
                    
                    z = y()
                    x = type(z)
                    """;
        var memory = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.String, "<class 'y'>");
        Assertions.AssertVariable(memory, "x", expected);
    }
}