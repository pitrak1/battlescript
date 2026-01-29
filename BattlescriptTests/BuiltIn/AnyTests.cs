using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class AnyTests
{
    [Test]
    public void EmptyListReturnsFalse()
    {
        var (callStack, closure) = Runner.Run("x = any([])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void AllTrueValuesReturnsTrue()
    {
        var (callStack, closure) = Runner.Run("x = any([True, True, True])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void OneTrueValueReturnsTrue()
    {
        var (callStack, closure) = Runner.Run("x = any([False, False, True])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void AllFalseValuesReturnsFalse()
    {
        var (callStack, closure) = Runner.Run("x = any([False, False, False])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }
}