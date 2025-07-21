using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class IntTests
{
    [Test]
    public void ValueIsNumericVariable()
    {
        var memory = Runner.Run("""
                                x = 0
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new NumericVariable(0));
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var memory = Runner.Run("""
                                x = 5
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new NumericVariable(5));
    }
    
    [Test]
    public void CanAdd()
    {
        var memory = Runner.Run("""
                                x = 5
                                y = 3
                                z = x + y
                                a = z.__value
                                """);
        Assertions.AssertVariable(memory, "a", new NumericVariable(8));
    }
    
    [Test]
    public void CanSubtract()
    {
        var memory = Runner.Run("""
                                x = 5
                                y = 3
                                z = x - y
                                a = z.__value
                                """);
        Assertions.AssertVariable(memory, "a", new NumericVariable(2));
    }
    
    [Test]
    public void CanMultiply()
    {
        var memory = Runner.Run("""
                                x = 5
                                y = 3
                                z = x * y
                                a = z.__value
                                """);
        Assertions.AssertVariable(memory, "a", new NumericVariable(15));
    }
}