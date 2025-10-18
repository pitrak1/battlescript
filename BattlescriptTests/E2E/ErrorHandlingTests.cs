using Battlescript;

namespace BattlescriptTests.E2ETests;

public class ErrorHandlingTests
{
    [TestFixture]
    public class Raise
    {
        [Test]
        public void CanRaiseBuiltInException()
        {
            var input = "raise TypeError('asdf')";
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }
        
        [Test]
        public void CanRaiseCustomException()
        {
            var input = """
                        class MyException(Exception):
                            pass
                            
                        raise MyException('asdf')
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("MyException"));
        }
    }

    [TestFixture]
    public class TryExcept
    {
        [Test]
        public void RunsMatchingBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except TypeError:
                            x = 12
                        """;
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 12));
        }
        
        [Test]
        public void DoesNotRunNonMatchingBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except AssertionError:
                            x = 12
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }

        [Test]
        public void AllowsMultipleExceptBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except AssertionError:
                            x = 9
                        except TypeError:
                            x = 12
                        """;
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 12));
        }
        
        [Test]
        public void OnlyRunsFirstMatchingExceptBlock()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except TypeError:
                            x = 9
                        except Exception:
                            x = 12
                        """;
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 9));
        }
        
        [Test]
        public void RunsElseIfAnyErrorIsRaised()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        else:
                            x = 9
                        """;
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 9));
        }
        
        [Test]
        public void RunsElseIfNoMatchingExceptBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except AssertionError:
                            x = 12
                        else:
                            x = 9
                        """;
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 9));
        }
    }

    [TestFixture]
    public class As
    {
        [Test]
        public void SupportsAsKeywordInExceptBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except TypeError as e:
                            x = e.message
                        """;
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "asdf"));
        }
    }
}