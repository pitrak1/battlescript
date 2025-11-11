using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class OperatorsTests
{
    [TestFixture]
    public class BooleanOperators
    {
        [Test]
        public void AndOperations()
        {
            var input = """
                        a = True and True
                        b = True and False
                        c = False and True
                        d = False and False
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "c", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "d", BsTypes.False);
        }
        
        [Test]
        public void OrOperations()
        {
            var input = """
                        a = True or True
                        b = True or False
                        c = False or True
                        d = False or False
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "c", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "d", BsTypes.False);
        }
        
        [Test]
        public void NotOperations()
        {
            var input = """
                        a = not False
                        b = not True
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "a", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "b", BsTypes.False);
        }
    }

    [TestFixture]
    public class IdentityOperators
    {
        [Test]
        public void IsOperations()
        {
            var input = """
                        class MyClass:
                            pass
                        
                        a = MyClass()
                        b = MyClass()
                        c = a
                        
                        d = a is b
                        e = a is c
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "d", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "e", BsTypes.True);
        }
        
        [Test]
        public void IsNotOperations()
        {
            var input = """
                        class MyClass:
                            pass

                        a = MyClass()
                        b = MyClass()
                        c = a

                        d = a is not b
                        e = a is not c
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "d", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "e", BsTypes.False);
        }
    }

    [TestFixture]
    public class BinaryOperators
    {
        [Test]
        public void Power()
        {
            var input = "x = 2 ** 3";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 8);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void Multiplication()
        {
            var input = "x = 2 * 3";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void TrueDivision()
        {
            var input = "x = 2 / 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 0.4);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void FloorDivision()
        {
            var input = "x = 7 // 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 1);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void Modulo()
        {
            var input = "x = 7 % 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 2);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void Addition()
        {
            var input = "x = 7 + 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 12);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void Subtraction()
        {
            var input = "x = 7 - 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 2);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class UnaryOperators
    {
        [Test]
        public void Positive()
        {
            var input = "x = +5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void Negative()
        {
            var input = "x = -5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, -5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class ComparisonOperators
    {
        [Test]
        public void Equals()
        {
            var input = """
                        x = 5 == 5
                        y = 5 == 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
        }
        
        [Test]
        public void NotEquals()
        {
            var input = """
                        x = 5 != 5
                        y = 5 != 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.True);
        }
        
        [Test]
        public void GreaterThan()
        {
            var input = """
                        x = 5 > 4
                        y = 5 > 5
                        z = 5 > 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "z", BsTypes.False);
        }
        
        [Test]
        public void GreaterThanOrEqualTo()
        {
            var input = """
                        x = 5 >= 4
                        y = 5 >= 5
                        z = 5 >= 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "z", BsTypes.False);
        }
        
        [Test]
        public void LessThan()
        {
            var input = """
                        x = 5 < 4
                        y = 5 < 5
                        z = 5 < 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "z", BsTypes.True);
        }
        
        [Test]
        public void LessThanOrEqualTo()
        {
            var input = """
                        x = 5 <= 4
                        y = 5 <= 5
                        z = 5 <= 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.False);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.True);
            Assertions.AssertVariable(callStack, closure, "z", BsTypes.True);
        }
    }

    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void Assignment()
        {
            var input = "x = 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AddAssignment()
        {
            var input = "x = 5\nx += 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 10);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SubtractionAssignment()
        {
            var input = "x = 5\nx -= 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 0);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void MultiplicationAssignment()
        {
            var input = "x = 5\nx *= 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 25);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void TrueDivisionAssignment()
        {
            var input = "x = 5\nx /= 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 2.5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void FloorDivisionAssignment()
        {
            var input = "x = 5\nx //= 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 2);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ModuloAssignment()
        {
            var input = "x = 5\nx %= 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 1);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void PowerAssignment()
        {
            var input = "x = 5\nx **= 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 25);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}