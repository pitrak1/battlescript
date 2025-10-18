using Battlescript;

namespace BattlescriptTests.Errors;

public class ValueErrorTests
{
    [Test]
    public void ListSliceStepCannotBeZero()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("x = [1, 2, 3, 4, 5]\ny = x[1:5:0]"));
        Assert.That(ex.Message, Is.EqualTo("slice step cannot be zero"));
        Assert.That(ex.Type, Is.EqualTo("ValueError"));
    }
    
    [Test]
    public void AssigningToListSliceStepCannotBeZero()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("x = [1, 2, 3, 4, 5]\nx[1:5:0] = [1]"));
        Assert.That(ex.Message, Is.EqualTo("slice step cannot be zero"));
        Assert.That(ex.Type, Is.EqualTo("ValueError"));
    }

    [Test]
    public void ListSizeMustEqualValueSizeWhenStepIsNotOne()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("x = [1, 2, 3, 4, 5]\nx[1:5:2] = [1]"));
        Assert.That(ex.Message, Is.EqualTo($"attempt to assign sequence of size 1 to extended slice of size 2"));
        Assert.That(ex.Type, Is.EqualTo("ValueError"));
    }
}