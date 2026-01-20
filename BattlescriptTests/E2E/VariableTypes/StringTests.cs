using Battlescript;

namespace BattlescriptTests.E2ETests;

public class StringTests
{
    [Test]
    public void SupportsStringVariablesUsingSingleQuotes()
    {
        var input = "x = 'asdf'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf");
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void SupportsStringVariablesUsingDoubleQuotes()
    {
        var input = "x = \"asdf\"";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf");
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void SupportsStringConcatenation()
    {
        var input = "x = 'asdf' + 'qwer'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdfqwer");
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
    
    [Test]
    public void SupportsMultipleStringConcatenation()
    {
        var input = "x = 'asdf' + 'qwer' + 'zxcv'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdfqwerzxcv");
        
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [TestFixture]
    public class StringInterpolation
    {
        [Test]
        public void InsertedVariable()
        {
            var input = """
                        x = 5
                        y = f"asdf{x}qwer"
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf5qwer");
            
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
        
        [Test]
        public void InsertedVariableAtStart()
        {
            var input = """
                        x = 5
                        y = f"{x}qwer"
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "5qwer");
            
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
            
        [Test]
        public void InsertedVariableAtEnd()
        {
            var input = """
                        x = 5
                        y = f"asdf{x}"
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf5");
            
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
            
        [Test]
        public void MultipleInsertedVariables()
        {
            var input = """
                        x = 5
                        y = 10
                        z = f"asdf{x}x{y}qwer"
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf5x10qwer");
            
            Assert.That(closure.GetVariable(callStack, "z"), Is.EqualTo(expected));
        }
            
        [Test]
        public void InsertedExpressions()
        {
            var input = """
                        x = 5
                        y = 10
                        z = f"asdf{x + y}qwer"
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf15qwer");
            
            Assert.That(closure.GetVariable(callStack, "z"), Is.EqualTo(expected));
        }
    }
}