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
            var (callStack, closure) = Runner.Run("""
                                    a = "asdf" == "asdf"
                                    b = "asdf" == "qwer"
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
        }

        [Test]
        public void Inequality()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = "asdf" != "asdf"
                                    b = "asdf" != "qwer"
                                    """);
            Assertions.AssertVariable(callStack, closure, "a", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
            Assertions.AssertVariable(callStack, closure, "b", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
        }

        [Test]
        public void Add()
        {
            var (callStack, closure) = Runner.Run("x = 'asdf' + 'qwer'");
            Assertions.AssertVariable(callStack, closure, "x",
                BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfqwer")));
        }

        [Test]
        public void Multiply()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 'asdf' * 2
                                    y = 3 * 'asdf'
                                    """);
            Assertions.AssertVariable(callStack, closure, "x",
                BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdf")));
            Assertions.AssertVariable(callStack, closure, "y",
                BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdfasdf")));
        }
    }

    [TestFixture]
    public class Truthiness
    {
        [Test]
        public void TrueIfNonEmpty()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = "asdf"
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void FalseIfEmpty()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = ""
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0)));
        }
    }

    [TestFixture]
    public class Conversions
    {
        [Test]
        public void InternalString()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_string__("hello")
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.String, new StringVariable("hello")));
        }

        [Test]
        public void String()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = "hello"
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.String, new StringVariable("hello")));
        }
        
        [Test]
        public void InternalNumeric()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_numeric__(1)
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.String, new StringVariable("1")));
        }
        
        [Test]
        public void Int()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 1
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.String, new StringVariable("1")));
        }
        
        [Test]
        public void Float()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 1.5
                                    y = str(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BtlTypes.Create(BtlTypes.Types.String, new StringVariable("1.5")));
        }
    }
}