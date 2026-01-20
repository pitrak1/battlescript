using Battlescript;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertStacktrace(List<StackFrame> input, List<StackFrame> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        for (var i = 0; i < input.Count; i++)
        {
            AssertStackFrame(input[i], expected[i]);
        }
    }

    public static void AssertStackFrame(StackFrame input, StackFrame expected)
    {
        Assert.That(input.File, Is.EqualTo(expected.File));
        Assert.That(input.Line, Is.EqualTo(expected.Line));
        Assert.That(input.Expression, Is.EqualTo(expected.Expression));
        Assert.That(input.Function, Is.EqualTo(expected.Function));
    }
}