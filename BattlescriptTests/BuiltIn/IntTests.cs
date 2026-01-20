using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class IntTests
{
    [Test]
    public void ValueIsNumericVariable()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 0
                                y = x.__btl_value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new NumericVariable(0));
    }
    
    [Test]
    public void ConstructorSetsValue()
    {
        var (callStack, closure) = Runner.Run("""
                                x = 5
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
            var (callStack, closure) = Runner.Run("x = 5 + 3");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(8)));
        }
    
        [Test]
        public void Subtract()
        {
            var (callStack, closure) = Runner.Run("x = 5 - 3");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(2)));
        }
    
        [Test]
        public void Multiply()
        {
            var (callStack, closure) = Runner.Run("x = 5 * 3");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(15)));
        }

        [Test]
        public void TrueDivide()
        {
            var (callStack, closure) = Runner.Run("x = 6 / 3");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivide()
        {
            var (callStack, closure) = Runner.Run("x = 9 // 2");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void Modulo()
        {
            var (callStack, closure) = Runner.Run("x = 9 % 2");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(1)));
        }
        
        [Test]
        public void Power()
        {
            var (callStack, closure) = Runner.Run("x = 2 ** 3");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(8)));
        }
        
        [Test]
        public void Negate()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    y = -x
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(-5)));
        }

        [Test]
        public void Positive()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    y = +x
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5)));
        }

        [Test]
        public void Equality()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5 == 5
                                    b = 5 == 6
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void Inequality()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5 != 5
                                    b = 5 != 6
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void LessThan()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5 < 4
                                    b = 5 < 5
                                    c = 5 < 6
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "c", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
        }

        [Test]
        public void LessThanOrEqual()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5 <= 4
                                    b = 5 <= 5
                                    c = 5 <= 6
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void GreaterThan()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5 > 4
                                    b = 5 > 5
                                    c = 5 > 6
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "c", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
        }
        [Test]
        public void GreaterThanOrEqual()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = 5 >= 4
                                    b = 5 >= 5
                                    c = 5 >= 6
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "c", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void AddAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    x += 3
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(8)));
        }
        
        [Test]
        public void SubtractAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    x -= 3
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(2)));
        }
        
        [Test]
        public void MultiplyAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    x *= 3
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(15)));
        }
        
        [Test]
        public void TrueDivideAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 6
                                    x /= 3
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivideAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 9
                                    x //= 2
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void ModuloAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 9
                                    x %= 2
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(1)));
        }
        
        [Test]
        public void PowerAssign()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 2
                                    x **= 3
                                    """);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(8)));
        }
    }

    public class Truthiness
    {
        [Test]
        public void TrueIfNonZero()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
        }

        [Test]
        public void FalseIfZero()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
        }
    }
    
    public class Conversions
    {
        [Test]
        public void InternalString()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_string__("1")
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(1)));
        }

        [Test]
        public void String()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = "5"
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5)));
        }
        
        [Test]
        public void InternalNumeric()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_numeric__(4)
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void Int()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 5.98
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Int, new NumericVariable(5)));
        }
    }
}