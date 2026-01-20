using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInIsSubclassTests
{
    [Test]
    public void ReturnsTrueIfFirstClassIsEqualToSecondClass()
    {
        var (callStack, closure) = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                y = issubclass(asdf, asdf)
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
    
    [Test]
    public void ReturnsTrueIfFirstClassInheritsFromSecondClass()
    {
        var (callStack, closure) = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer(asdf):
                                    j = 6
                                    
                                y = issubclass(qwer, asdf)
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
    
    [Test]
    public void ReturnsFalseIfFirstClassDoesNotInheritFromSecondClass()
    {
        var (callStack, closure) = Runner.Run("""
                                class asdf:
                                    i = 5
                                    
                                class qwer(asdf):
                                    j = 6
                                    
                                y = issubclass(asdf, qwer)
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
}