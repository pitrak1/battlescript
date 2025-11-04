using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class ListTests
{
    [Test]
    public void ValueIsSequenceVariable()
    {
        var (callStack, closure) = Runner.Run("""
                                x = []
                                y = x.__value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new SequenceVariable());
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                y = x.__value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new SequenceVariable([
            BsTypes.Create(BsTypes.Types.Int, 1),
            BsTypes.Create(BsTypes.Types.Int, 2),
            BsTypes.Create(BsTypes.Types.Int, 3),
        ]));
    }
    
    [Test]
    public void CanGetItem()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                y = x[1]
                                """);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 2));
    }

    [Test]
    public void CanSetItem()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                x[1] = 4
                                y = x[1]
                                """);
        Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 4));
    }
}