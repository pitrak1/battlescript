using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class AbsTests
{
    [Test]
    public void PositiveIntReturnsItself()
    {
        var (callStack, closure) = Runner.Run("x = abs(5)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
    }

    [Test]
    public void NegativeIntReturnsPositive()
    {
        var (callStack, closure) = Runner.Run("x = abs(-5)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5))));
    }

    [Test]
    public void ZeroReturnsZero()
    {
        var (callStack, closure) = Runner.Run("x = abs(0)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(0))));
    }
    
    [Test]
    public void PositiveFloatReturnsItself()
    {
        var (callStack, closure) = Runner.Run("x = abs(5.5)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(5.5))));
    }

    [Test]
    public void NegativeFloatReturnsPositive()
    {
        var (callStack, closure) = Runner.Run("x = abs(-5.5)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(5.5))));
    }

    [Test]
    public void ZeroFloatReturnsZero()
    {
        var (callStack, closure) = Runner.Run("x = abs(0.0)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(0.0))));
    }

    [Test]
    public void UserCreatedClass()
    {
        var input = """
                    class MyNum:
                        def __init__(self, val):
                            self.val = val
                        def __abs__(self):
                            return self.val * -1

                    m = MyNum(-10)
                    x = abs(m)
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(10))));
    }
    
    [Test]
    public void ThrowsTypeErrorForString()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("x = abs('hello')"));
        Assert.That(ex.Type, Is.EqualTo("TypeError"));
    }

    [Test]
    public void ThrowsTypeErrorForWrongArgCount()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("x = abs(1, 2)"));
        Assert.That(ex.Type, Is.EqualTo("TypeError"));
    }
}