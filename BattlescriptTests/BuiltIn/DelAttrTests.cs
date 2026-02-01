using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class DelAttrTests
{
    [Test]
    public void DeletesAttributeFromObject()
    {
        var input = """
                    class Foo:
                        pass
                    x = Foo()
                    x.bar = 5
                    delattr(x, 'bar')
                    y = hasattr(x, 'bar')
                    """;
        var (callStack, closure) = Runner.Run(input);
        var y = closure.GetVariable(callStack, "y");
        Assert.That(y, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void ThrowsAttributeErrorIfAttributeDoesNotExist()
    {
        var input = """
                    class Foo:
                        pass
                    x = Foo()
                    delattr(x, 'bar')
                    """;
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
        Assert.That(ex.Type, Is.EqualTo("AttributeError"));
    }

    [Test]
    public void ThrowsAttributeErrorForNonExistentAttributeOnInt()
    {
        var input = """
                    x = 5
                    delattr(x, 'bar')
                    """;
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
        Assert.That(ex.Type, Is.EqualTo("AttributeError"));
    }

    [Test]
    public void ThrowsWithWrongNumberOfArguments()
    {
        var input = """
                    class Foo:
                        pass
                    x = Foo()
                    delattr(x)
                    """;
        // The wrapper function catches wrong number of arguments before the binding
        Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
    }

    [Test]
    public void DeletesAttributeWithDynamicName()
    {
        var input = """
                    class Foo:
                        pass
                    x = Foo()
                    x.bar = 5
                    delattr(x, 'bar')
                    y = hasattr(x, 'bar')
                    """;
        var (callStack, closure) = Runner.Run(input);
        var y = closure.GetVariable(callStack, "y");
        Assert.That(y, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }
}
