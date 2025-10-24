using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public class BuiltInLenTests
{
    [Test]
    public void EmptyString()
    {
        var memory = Runner.Run("""
                                x = len("")
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Int, 0);
        Assertions.AssertVariable(memory, "x", expected);
    }

    [Test]
    public void NonEmptyString()
    {
        var memory = Runner.Run("""
                                x = len("asdf")
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Int, 4);
        Assertions.AssertVariable(memory, "x", expected);
    }

    [Test]
    public void AllowsTokensAfterFunctionCall()
    {
        var memory = Runner.Run("""
                                x = len("asdf") + 1
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Int, 5);
        Assertions.AssertVariable(memory, "x", expected);
    }
    
    [Test]
    public void AllowsResultToBeUsedInExpressions()
    {
        var memory = Runner.Run("""
                                x = 1 + len("asdf")
                                """);
        var expected = BsTypes.Create(BsTypes.Types.Int, 5);
        Assertions.AssertVariable(memory, "x", expected);
    }
}