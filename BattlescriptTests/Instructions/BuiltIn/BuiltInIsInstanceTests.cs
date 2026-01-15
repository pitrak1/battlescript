using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInIsInstanceTests
{
    [Test]
    public void ReturnsTrueIfObjectIsDirectInstanceOfClass()
    {
        var (callStack, closure) = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                x = asdf()
                                y = isinstance(x, asdf)
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Bool, true);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
    
    [Test]
    public void ReturnsTrueIfObjectIsInheritedInstanceOfClass()
    {
        var (callStack, closure) = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer(asdf):
                                    j = 6
                                    
                                x = qwer()
                                y = isinstance(x, asdf)
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Bool, true);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }

    [Test]
    public void ReturnsFalseIfObjectIsNotInstanceOfClass()
    {
        var (callStack, closure) = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer:
                                    j = 6
                                    
                                x = qwer()
                                y = isinstance(x, asdf)
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Bool, false);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }

    [Test]
    public void ReturnsTrueIfObjectIsSameTransitionType()
    {
        var (callStack, closure) = Runner.Run("""
                                              x = __btl_numeric__(5)
                                              y = isinstance(x, __btl_numeric__)
                                              """);
        var expected = BsTypes.Create(BsTypes.Types.Bool, true);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
    
    [Test]
    public void ReturnsFalseIfObjectIsDifferentTransitionType()
    {
        var (callStack, closure) = Runner.Run("""
                                              x = __btl_numeric__(5)
                                              y = isinstance(x, __btl_string__)
                                              """);
        var expected = BsTypes.Create(BsTypes.Types.Bool, false);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
}