using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class FloatTests
{
    [Test]
    public void ValueIsNumericVariable()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 0.0
                                y = x.__btl_value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new NumericVariable(0));
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 5.0
                                y = x.__btl_value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new NumericVariable(5));
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void Add()
        {
            var (callStack, closure) = Runner.Run("x = 5.0 + 3.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(8)));
        }
    
        [Test]
        public void Subtract()
        {
            var (callStack, closure) = Runner.Run("x = 5.0 - 3.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(2)));
        }
    
        [Test]
        public void Multiply()
        {
            var (callStack, closure) = Runner.Run("x = 5.0 * 3.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(15)));
        }
        
        [Test]
        public void TrueDivide()
        {
            var (callStack, closure) = Runner.Run("x = 6.0 / 3.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivide()
        {
            var (callStack, closure) = Runner.Run("x = 9.0 // 2.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void Modulo()
        {
            var (callStack, closure) = Runner.Run("x = 9.0 % 2.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(1)));
        }
        
        [Test]
        public void Power()
        {
            var (callStack, closure) = Runner.Run("x = 2.0 ** 3.0");
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(8)));
        }
        
        [Test]
        public void Negate()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.0
                                    y = -x
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(-5)));
        }
        
        [Test]
        public void Positive()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.0
                                    y = +x
                                    """);
        }
        
        [Test]
        public void Equality()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5.0 == 5.0
                                    b = 5.0 == 6.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void Inequality()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5.0 != 5.0
                                    b = 5.0 != 6.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void LessThan()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5.0 < 4.0
                                    b = 5.0 < 5.0
                                    c = 5.0 < 6.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "c", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }

        [Test]
        public void LessThanOrEqual()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5.0 <= 4.0
                                    b = 5.0 <= 5.0
                                    c = 5.0 <= 6.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "c", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }

        [Test]
        public void GreaterThan()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5.0 > 4.0
                                    b = 5.0 > 5.0
                                    c = 5.0 > 6.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "c", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void GreaterThanOrEqual()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5.0 >= 4.0
                                    b = 5.0 >= 5.0
                                    c = 5.0 >= 6.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "c", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void AddAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.0
                                    x += 3.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(8)));
        }
        
        [Test]
        public void SubtractAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.0
                                    x -= 3.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(2)));
        }
        
        [Test]
        public void MultiplyAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.0
                                    x *= 3.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(15)));
        }
        
        [Test]
        public void TrueDivideAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 6.0
                                    x /= 3.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivideAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 9.0
                                    x //= 2.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void ModuloAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 9.0
                                    x %= 2.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(1)));
        }
        
        [Test]
        public void PowerAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 2.0
                                    x **= 3.0
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(8)));
        }
    }

    public class Truthiness
    {
        [Test]
        public void TrueIfNonZero()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void FalseIfZero()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 0.0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
    }
    
    public class Conversions
    {
        [Test]
        public void InternalString()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_string__("1.45")
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(1.45)));
        }

        [Test]
        public void String()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = "1.69"
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(1.69)));
        }
        
        [Test]
        public void InternalNumeric()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_numeric__(1.45)
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(1.45)));
        }
        
        [Test]
        public void Int()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    y = float(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Float, new NumericVariable(5.0)));
        }
    }
}