using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class FloatTests
{
    [Test]
    public void ValueIsNumericVariable()
    {
        var memory = Runner.Run("""
                                x = 0.0
                                y = x.__value
                                """);
        Assert.That(memory.Scopes[0]["y"], Is.EqualTo(new NumericVariable(0)));
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var memory = Runner.Run("""
                                x = 5.0
                                y = x.__value
                                """);
        Assert.That(memory.Scopes[0]["y"], Is.EqualTo(new NumericVariable(5)));
    }
    
    [Test]
    public void CanAdd()
    {
        var memory = Runner.Run("""
                                x = 5.0
                                y = 3.0
                                z = x + y
                                a = z.__value
                                """);
        Assert.That(memory.Scopes[0]["a"], Is.EqualTo(new NumericVariable(8)));
    }
    
    [Test]
    public void CanSubtract()
    {
        var memory = Runner.Run("""
                                x = 5.0
                                y = 3.0
                                z = x - y
                                a = z.__value
                                """);
        Assert.That(memory.Scopes[0]["a"], Is.EqualTo(new NumericVariable(2)));
    }
    
    [Test]
    public void CanMultiply()
    {
        var memory = Runner.Run("""
                                x = 5.0
                                y = 3.0
                                z = x * y
                                a = z.__value
                                """);
        Assert.That(memory.Scopes[0]["a"], Is.EqualTo(new NumericVariable(15)));
    }
}