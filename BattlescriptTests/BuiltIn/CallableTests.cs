using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class CallableTests
{
    [Test]
    public void FunctionIsCallable()
    {
        var input = """
                    def foo():
                        pass
                    x = callable(foo)
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void LambdaIsCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(lambda: 1)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ClassIsCallable()
    {
        var input = """
                    class Foo:
                        pass
                    x = callable(Foo)
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void InstanceWithCallMethodIsCallable()
    {
        var input = """
                    class Foo:
                        def __call__(self):
                            return 42
                    obj = Foo()
                    x = callable(obj)
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void InstanceWithoutCallMethodIsNotCallable()
    {
        var input = """
                    class Foo:
                        pass
                    obj = Foo()
                    x = callable(obj)
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void BoundMethodIsCallable()
    {
        var input = """
                    class Foo:
                        def bar(self):
                            pass
                    obj = Foo()
                    x = callable(obj.bar)
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void IntIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(42)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void FloatIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(3.14)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void StringIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable('hello')");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void ListIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable([1, 2, 3])");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void DictIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable({'a': 1})");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void NoneIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(None)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void BoolIsNotCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(True)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void BuiltInFunctionIsCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(len)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void BuiltInTypeIsCallable()
    {
        var (callStack, closure) = Runner.Run("x = callable(int)");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }
}
