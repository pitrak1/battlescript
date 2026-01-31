using Battlescript;

namespace BattlescriptTests.Interpreter;

[TestFixture]
public class ObjectComparisonTests
{
    [Test]
    public void ListsCanBeComparedForEquality()
    {
        var input = """
                    a = [1, 2, 3]
                    b = [1, 2, 3]
                    result = a == b
                    """;
        var (callStack, closure) = Runner.Run(input);
        var result = closure.GetVariable(callStack, "result");
        Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void ListsCanBeComparedForInequality()
    {
        var input = """
                    a = [1, 2, 3]
                    b = [1, 2, 4]
                    result = a == b
                    """;
        var (callStack, closure) = Runner.Run(input);
        var result = closure.GetVariable(callStack, "result");
        Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
    }

    [Test]
    public void ListsCanUseNotEquals()
    {
        var input = """
                    a = [1, 2, 3]
                    b = [1, 2, 4]
                    result = a != b
                    """;
        var (callStack, closure) = Runner.Run(input);
        var result = closure.GetVariable(callStack, "result");
        Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void CustomObjectsCanBeCompared()
    {
        var input = """
                    class Point:
                        def __init__(self, x, y):
                            self.x = x
                            self.y = y
                        def __eq__(self, other):
                            return self.x == other.x and self.y == other.y

                    p1 = Point(1, 2)
                    p2 = Point(1, 2)
                    result = p1 == p2
                    """;
        var (callStack, closure) = Runner.Run(input);
        var result = closure.GetVariable(callStack, "result");
        Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void CustomObjectsWithoutEqRaiseError()
    {
        var input = """
                    class Foo:
                        pass
                    a = Foo()
                    b = Foo()
                    result = a == b
                    """;
        Assert.Throws<InterpreterInvalidOperationException>(() => Runner.Run(input));
    }

    [Test]
    public void StringsCanBeCompared()
    {
        var input = """
                    a = "hello"
                    b = "hello"
                    result = a == b
                    """;
        var (callStack, closure) = Runner.Run(input);
        var result = closure.GetVariable(callStack, "result");
        Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }

    [Test]
    public void DictsCanBeCompared()
    {
        var input = """
                    a = {'x': 1}
                    b = {'x': 1}
                    result = a == b
                    """;
        var (callStack, closure) = Runner.Run(input);
        var result = closure.GetVariable(callStack, "result");
        Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
    }
}
