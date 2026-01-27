using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class HasAttrTests
{
    [Test]
    public void ReturnsTrueForExistingMethod()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 5
                                y = hasattr(x, "__abs__")
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ReturnsFalseForMissingAttribute()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 5
                                y = hasattr(x, "nonexistent")
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void ReturnsTrueForUserDefinedAttribute()
    {
        var input = """
                    class Foo:
                        def __init__(self):
                            self.bar = 42

                    f = Foo()
                    x = hasattr(f, "bar")
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ReturnsFalseForMissingUserDefinedAttribute()
    {
        var input = """
                    class Foo:
                        def __init__(self):
                            self.bar = 42

                    f = Foo()
                    x = hasattr(f, "baz")
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void ReturnsTrueForUserDefinedMethod()
    {
        var input = """
                    class Foo:
                        def greet(self):
                            return "hello"

                    f = Foo()
                    x = hasattr(f, "greet")
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ReturnsTrueForDunderMethodOnString()
    {
        var (callStack, closure) = Runner.Run("""
                                s = "hello"
                                x = hasattr(s, "__len__")
                                """);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ReturnsFalseForAbsOnString()
    {
        var (callStack, closure) = Runner.Run("""
                                s = "hello"
                                x = hasattr(s, "__abs__")
                                """);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void ReturnsTrueForDunderMethodOnFloat()
    {
        var (callStack, closure) = Runner.Run("""
                                f = 3.14
                                x = hasattr(f, "__add__")
                                """);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ReturnsTrueForDunderMethodOnList()
    {
        var (callStack, closure) = Runner.Run("""
                                lst = [1, 2, 3]
                                x = hasattr(lst, "__len__")
                                """);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ReturnsTrueForInheritedAttribute()
    {
        var input = """
                    class Base:
                        def foo(self):
                            return 1

                    class Child(Base):
                        pass

                    c = Child()
                    x = hasattr(c, "foo")
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }
}
