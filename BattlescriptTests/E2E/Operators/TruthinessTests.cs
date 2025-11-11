using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class TruthinessTests
{
    [Test]
    public void Booleans()
    {
        var input = """
                    x = bool(True)
                    y = bool(False)
                    z = bool(None)
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
        Assertions.AssertVariable(callStack, closure, "z", BsTypes.False);
    }
    
    [Test]
    public void Ints()
    {
        var input = """
                    x = bool(1)
                    y = bool(0)
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
    }
    
    [Test]
    public void Floats()
    {
        var input = """
                    x = bool(1.9)
                    y = bool(0.0)
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
    }
    
    [Test]
    public void Strings()
    {
        var input = """
                    x = bool("asdf")
                    y = bool("")
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
    }
    
    [Test]
    public void Lists()
    {
        var input = """
                    x = bool([1, 2, 3])
                    y = bool([])
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
    }
    
    [Test]
    public void Dictionaries()
    {
        var input = """
                    x = bool({1: 2, 3: 4})
                    y = bool({})
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
    }

    [Test]
    public void Functions()
    {
        var input = """
                    def func():
                        pass
                    
                    x = bool(func)
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
    }
    
    [Test]
    public void Class()
    {
        var input = """
                    class MyClass:
                        pass

                    x = bool(MyClass)
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
    }
    
    [Test]
    public void Objects()
    {
        var input = """
                    class MyClass:
                        pass
                        
                    y = MyClass()
                    x = bool(y)
                    """;
        var (callStack, closure) = Runner.Run(input);
            
        Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
    }
}