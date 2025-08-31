using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class StrTests
{
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