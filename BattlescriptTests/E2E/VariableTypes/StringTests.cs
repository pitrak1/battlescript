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
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsStringVariablesUsingDoubleQuotes()
    {
        var input = "x = \"asdf\"";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf");
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void SupportsStringConcatenation()
    {
        var input = "x = 'asdf' + 'qwer'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdfqwer");
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsMultipleStringConcatenation()
    {
        var input = "x = 'asdf' + 'qwer' + 'zxcv'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdfqwerzxcv");
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
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
            
            Assertions.AssertVariable(callStack, closure, "y", expected);
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
            
            Assertions.AssertVariable(callStack, closure, "y", expected);
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
            
            Assertions.AssertVariable(callStack, closure, "y", expected);
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
            
            Assertions.AssertVariable(callStack, closure, "z", expected);
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
            
            Assertions.AssertVariable(callStack, closure, "z", expected);
        }
    }
}