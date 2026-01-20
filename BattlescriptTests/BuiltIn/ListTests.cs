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
                                y = x.__btl_value
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(new SequenceVariable()));
    }

    [Test]
    public void ConstructorSetsValue()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                y = x.__btl_value
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(new SequenceVariable([
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
        ])));
    }

    [Test]
    public void CanGetItem()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                y = x[1]
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
    }

    [Test]
    public void CanSetItem()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                x[1] = 4
                                y = x[1]
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 4)));
    }
}
