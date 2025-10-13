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

    [TestFixture]
    public class Operators
    {
        [Test]
        public void Add()
        {
            var memory = Runner.Run("x = 5 + 3");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(8)));
        }
    
        [Test]
        public void Subtract()
        {
            var memory = Runner.Run("x = 5 - 3");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(2)));
        }
    
        [Test]
        public void Multiply()
        {
            var memory = Runner.Run("x = 5 * 3");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(15)));
        }

        [Test]
        public void TrueDivide()
        {
            var memory = Runner.Run("x = 6 / 3");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivide()
        {
            var memory = Runner.Run("x = 9 // 2");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void Modulo()
        {
            var memory = Runner.Run("x = 9 % 2");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(1)));
        }
        
        [Test]
        public void Power()
        {
            var memory = Runner.Run("x = 2 ** 3");
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(8)));
        }
        
        [Test]
        public void Negate()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    y = -x
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, new NumericVariable(-5)));
        }

        [Test]
        public void Positive()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    y = +x
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, new NumericVariable(5)));
        }

        [Test]
        public void Equality()
        {
            var memory = Runner.Run("""
                                    a = 5 == 5
                                    b = 5 == 6
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void Inequality()
        {
            var memory = Runner.Run("""
                                    a = 5 != 5
                                    b = 5 != 6
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void LessThan()
        {
            var memory = Runner.Run("""
                                    a = 5 < 4
                                    b = 5 < 5
                                    c = 5 < 6
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }

        [Test]
        public void LessThanOrEqual()
        {
            var memory = Runner.Run("""
                                    a = 5 <= 4
                                    b = 5 <= 5
                                    c = 5 <= 6
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void GreaterThan()
        {
            var memory = Runner.Run("""
                                    a = 5 > 4
                                    b = 5 > 5
                                    c = 5 > 6
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
        [Test]
        public void GreaterThanOrEqual()
        {
            var memory = Runner.Run("""
                                    a = 5 >= 4
                                    b = 5 >= 5
                                    c = 5 >= 6
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "c", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void AddAssign()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    x += 3
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(8)));
        }
        
        [Test]
        public void SubtractAssign()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    x -= 3
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(2)));
        }
        
        [Test]
        public void MultiplyAssign()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    x *= 3
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(15)));
        }
        
        [Test]
        public void TrueDivideAssign()
        {
            var memory = Runner.Run("""
                                    x = 6
                                    x /= 3
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Float, new NumericVariable(2.0)));
        }
        
        [Test]
        public void FloorDivideAssign()
        {
            var memory = Runner.Run("""
                                    x = 9
                                    x //= 2
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void ModuloAssign()
        {
            var memory = Runner.Run("""
                                    x = 9
                                    x %= 2
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(1)));
        }
        
        [Test]
        public void PowerAssign()
        {
            var memory = Runner.Run("""
                                    x = 2
                                    x **= 3
                                    """);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, new NumericVariable(8)));
        }
    }

    public class Truthiness
    {
        [Test]
        public void TrueIfNonZero()
        {
            var memory = Runner.Run("""
                                    x = 5
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }

        [Test]
        public void FalseIfZero()
        {
            var memory = Runner.Run("""
                                    x = 0
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
                                    x = __string__("1")
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, new NumericVariable(1)));
        }

        [Test]
        public void String()
        {
            var memory = Runner.Run("""
                                    x = "5"
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, new NumericVariable(5)));
        }
        
        [Test]
        public void InternalNumeric()
        {
            var memory = Runner.Run("""
                                    x = __numeric__(4)
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, new NumericVariable(4)));
        }
        
        [Test]
        public void Int()
        {
            var memory = Runner.Run("""
                                    x = 5.98
                                    y = int(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, new NumericVariable(5)));
        }
    }
}