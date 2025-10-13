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

    [TestFixture]
    public class Operators
    {
        [Test]
        public void Add()
        {
            var memory = Runner.Run("x = 5.0 + 3.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(8)));
        }
    
        [Test]
        public void Subtract()
        {
            var memory = Runner.Run("x = 5.0 - 3.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(2)));
        }
    
        [Test]
        public void Multiply()
        {
            var memory = Runner.Run("x = 5.0 * 3.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(15)));
        }
        
        [Test]
        public void TrueDivide()
        {
            var memory = Runner.Run("x = 6.0 / 3.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivide()
        {
            var memory = Runner.Run("x = 9.0 // 2.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void Modulo()
        {
            var memory = Runner.Run("x = 9.0 % 2.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(1)));
        }
        
        [Test]
        public void Power()
        {
            var memory = Runner.Run("x = 2.0 ** 3.0");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(8)));
        }
        
        [Test]
        public void Negate()
        {
            var memory = Runner.Run("""
                                    x = 5.0
                                    y = -x
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Float, new NumericVariable(-5)));
        }
        
        [Test]
        public void Positive()
        {
            var memory = Runner.Run("""
                                    x = 5.0
                                    y = +x
                                    """);
        }
        
        [Test]
        public void Equality()
        {
            var memory = Runner.Run("""
                                    a = 5.0 == 5.0
                                    b = 5.0 == 6.0
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void Inequality()
        {
            var memory = Runner.Run("""
                                    a = 5.0 != 5.0
                                    b = 5.0 != 6.0
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void LessThan()
        {
            var memory = Runner.Run("""
                                    a = 5.0 < 4.0
                                    b = 5.0 < 5.0
                                    c = 5.0 < 6.0
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }

        [Test]
        public void LessThanOrEqual()
        {
            var memory = Runner.Run("""
                                    a = 5.0 <= 4.0
                                    b = 5.0 <= 5.0
                                    c = 5.0 <= 6.0
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }

        [Test]
        public void GreaterThan()
        {
            var memory = Runner.Run("""
                                    a = 5.0 > 4.0
                                    b = 5.0 > 5.0
                                    c = 5.0 > 6.0
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void GreaterThanOrEqual()
        {
            var memory = Runner.Run("""
                                    a = 5.0 >= 4.0
                                    b = 5.0 >= 5.0
                                    c = 5.0 >= 6.0
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void AddAssign()
        {
            var memory = Runner.Run("""
                                    x = 5.0
                                    x += 3.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(8)));
        }
        
        [Test]
        public void SubtractAssign()
        {
            var memory = Runner.Run("""
                                    x = 5.0
                                    x -= 3.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(2)));
        }
        
        [Test]
        public void MultiplyAssign()
        {
            var memory = Runner.Run("""
                                    x = 5.0
                                    x *= 3.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(15)));
        }
        
        [Test]
        public void TrueDivideAssign()
        {
            var memory = Runner.Run("""
                                    x = 6.0
                                    x /= 3.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivideAssign()
        {
            var memory = Runner.Run("""
                                    x = 9.0
                                    x //= 2.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void ModuloAssign()
        {
            var memory = Runner.Run("""
                                    x = 9.0
                                    x %= 2.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(1)));
        }
        
        [Test]
        public void PowerAssign()
        {
            var memory = Runner.Run("""
                                    x = 2.0
                                    x **= 3.0
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(8)));
        }
    }

    public class Truthiness
    {
        [Test]
        public void TrueIfNonZero()
        {
            var memory = Runner.Run("""
                                    x = 5.0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void FalseIfZero()
        {
            var memory = Runner.Run("""
                                    x = 0.0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
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