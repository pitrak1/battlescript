using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class StrTests
{
    [TestFixture]
    public class Operators
    {
        [Test]
        public void Equality()
        {
            var memory = Runner.Run("""
                                    a = "asdf" == "asdf"
                                    b = "asdf" == "qwer"
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }

        [Test]
        public void Inequality()
        {
            var memory = Runner.Run("""
                                    a = "asdf" != "asdf"
                                    b = "asdf" != "qwer"
                                    """);
            Assertions.AssertVariable(memory, "a", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(memory, "b", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }

        [Test]
        public void Add()
        {
            var memory = Runner.Run("x = 'asdf' + 'qwer'");
            Assertions.AssertVariable(memory, "x",
                memory.Create(Memory.BsTypes.String, new StringVariable("asdfqwer")));
        }

        [Test]
        public void Multiply()
        {
            var memory = Runner.Run("""
                                    x = 'asdf' * 2
                                    y = 3 * 'asdf'
                                    """);
            Assertions.AssertVariable(memory, "x",
                memory.Create(Memory.BsTypes.String, new StringVariable("asdfasdf")));
            Assertions.AssertVariable(memory, "y",
                memory.Create(Memory.BsTypes.String, new StringVariable("asdfasdfasdf")));
        }
    }

    [TestFixture]
    public class Truthiness
    {
        [Test]
        public void TrueIfNonEmpty()
        {
            var memory = Runner.Run("""
                                    x = "asdf"
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void FalseIfEmpty()
        {
            var memory = Runner.Run("""
                                    x = ""
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Bool, new NumericVariable(0)));
        }
    }

    [TestFixture]
    public class Conversions
    {
        [Test]
        public void InternalString()
        {
            var memory = Runner.Run("""
                                    x = __string__("hello")
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.String, new StringVariable("hello")));
        }

        [Test]
        public void String()
        {
            var memory = Runner.Run("""
                                    x = "hello"
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.String, new StringVariable("hello")));
        }
        
        [Test]
        public void InternalNumeric()
        {
            var memory = Runner.Run("""
                                    x = __numeric__(1)
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.String, new StringVariable("1")));
        }
        
        [Test]
        public void Int()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.String, new StringVariable("1")));
        }
        
        [Test]
        public void Float()
        {
            var memory = Runner.Run("""
                                    x = 1.5
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.String, new StringVariable("1.5")));
        }
    }
}