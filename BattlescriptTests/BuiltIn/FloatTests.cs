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
        Assertions.AssertVariable(memory, "y", new NumericVariable(0));
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var memory = Runner.Run("""
                                x = 5.0
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new NumericVariable(5));
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
        Assertions.AssertVariable(memory, "a", new NumericVariable(8));
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
        Assertions.AssertVariable(memory, "a", new NumericVariable(2));
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
        Assertions.AssertVariable(memory, "a", new NumericVariable(15));
    }
    
    public class Conversions
    {
        [Test]
        public void InternalString()
        {
            var memory = Runner.Run("""
                                    x = __string__("1.45")
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Float, new NumericVariable(1.45)));
        }

        [Test]
        public void String()
        {
            var memory = Runner.Run("""
                                    x = "1.69"
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Float, new NumericVariable(1.69)));
        }
        
        [Test]
        public void InternalNumeric()
        {
            var memory = Runner.Run("""
                                    x = __numeric__(1.45)
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Float, new NumericVariable(1.45)));
        }
        
        [Test]
        public void Int()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Float, new NumericVariable(5.0)));
        }
    }
}