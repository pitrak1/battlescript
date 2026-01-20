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
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1))));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0))));
        }

        [Test]
        public void Inequality()
        {
            var (callStack, closure) = Runner.Run("""
                                    a = "asdf" != "asdf"
                                    b = "asdf" != "qwer"
                                    """);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0))));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1))));
        }

        [Test]
        public void Add()
        {
            var (callStack, closure) = Runner.Run("x = 'asdf' + 'qwer'");
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfqwer"))));
        }

        [Test]
        public void Multiply()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 'asdf' * 2
                                    y = 3 * 'asdf'
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdf"))));
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("asdfasdfasdf"))));
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
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(1))));
        }

        [Test]
        public void FalseIfEmpty()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = ""
                                    y = bool(x)
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, new NumericVariable(0))));
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
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("hello"))));
        }

        [Test]
        public void String()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = "hello"
                                    y = str(x)
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("hello"))));
        }

        [Test]
        public void InternalNumeric()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = __btl_numeric__(1)
                                    y = str(x)
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("1"))));
        }

        [Test]
        public void Int()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 1
                                    y = str(x)
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("1"))));
        }

        [Test]
        public void Float()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 1.5
                                    y = str(x)
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, new StringVariable("1.5"))));
        }
    }
}
