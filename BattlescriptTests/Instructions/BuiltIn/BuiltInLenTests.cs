using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInLenTests
{
    [Test]
    public void EmptyString()
    {
        var (callStack, closure) = Runner.Run("""
                                x = len("")
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, 0);
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void NonEmptyString()
    {
        var (callStack, closure) = Runner.Run("""
                                x = len("asdf")
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, 4);
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void AllowsTokensAfterFunctionCall()
    {
        var (callStack, closure) = Runner.Run("""
                                x = len("asdf") + 1
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void AllowsResultToBeUsedInExpressions()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 1 + len("asdf")
                                """);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}