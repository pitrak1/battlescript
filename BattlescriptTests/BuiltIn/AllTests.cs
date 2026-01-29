using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class AllTests
{
    [Test]
    public void EmptyListReturnsTrue()
    {
        var (callStack, closure) = Runner.Run("x = all([])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void AllTrueValuesReturnsTrue()
    {
        var (callStack, closure) = Runner.Run("x = all([True, True, True])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void OneFalseValueReturnsFalse()
    {
        var (callStack, closure) = Runner.Run("x = all([True, False, True])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void AllFalseValuesReturnsFalse()
    {
        var (callStack, closure) = Runner.Run("x = all([False, False, False])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }
}
