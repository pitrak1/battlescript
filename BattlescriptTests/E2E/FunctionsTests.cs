using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class FunctionsTests
{
    [TestFixture]
    public class BasicFunctionDefinitions
    {
        [Test]
        public void HandlesFunctionDefinitionWithNoArguments()
        {
            var input = """
                        def func():
                            x = 5
                        """;
            var expected = new FunctionVariable(
                "func",
                [],
                [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new NumericInstruction(5))
                ]);
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "func", expected);
        }
    
        [Test]
        public void HandlesFunctionDefinitionWithOneArgument()
        {
            var input = """
                        def func(asdf):
                            x = asdf
                        """;
            var expected = new FunctionVariable(
                "func",
                [new VariableInstruction("asdf")],
                [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new VariableInstruction("asdf"))
                ]);
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "func", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithMultipleArguments()
        {
            var input = """
                        def func(asdf, qwer):
                            x = asdf
                        """;
            var expected = new FunctionVariable(
                "func",
                [
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                ],
                [
                    new AssignmentInstruction(
                        operation: "=", 
                        left: new VariableInstruction("x"),
                        right: new VariableInstruction("asdf"))
                ]);
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "func", expected);
        }
    }
    
    [TestFixture]
    public class BasicFunctionCalls
    {
        [Test]
        public void HandlesFunctionCallWithNoArguments()
        {
            var input = """
                        x = 6
                        def func():
                            x = 5
                        func()
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 5);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesFunctionCallWithArguments()
        {
            var input = """
                        x = 6
                        def func(y, z):
                            x = y + z
                        func(2, 3)
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 5);
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
    
    [TestFixture]
    public class ReturnValues
    {
        [Test]
        public void HandlesBasicValues()
        {
            var input = """
                        def func():
                            return 15
                        x = func()
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 15);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesExpressions()
        {
            var input = """
                        def func(y, z):
                            return y + z
                        x = func(4, 8)
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 12);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void StopsExecution()
        {
            var input = """
                        x = 4
                        def func():
                            x = 5
                            return
                            x = 6
                        func()
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 5);
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
    
    [TestFixture]
    public class DefaultArguments
    {
        [Test]
        public void AllowsDefiningDefaultArguments()
        {
            var input = """
                        def func(y = 5):
                            return y
                        x = func(6)
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 6);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void UsesDefaultArgumentsWhenArgumentNotProvided()
        {
            var input = """
                        def func(y = 5):
                            return y
                        x = func()
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 5);
            Assertions.AssertVariable(memory, "x", expected);
        }
    
        [Test]
        public void DefaultArgumentsMustBeAfterRequiredArguments()
        {
            var input = """
                        def func(y = 5, z):
                            return y + z
                        """;
            Assert.Throws<Exception>(() => Runner.Run(input));
        }
    }
    
    [TestFixture]
    public class KeywordArguments
    {
        [Test]
        public void SupportsBasicKeywordArguments()
        {
            var input = """
                        def func(y = 5):
                            return y
                        x = func(y = 6)
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 6);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsMixedPositionalAndKeywordArguments()
        {
            var input = """
                        def func(x, y = 5):
                            return x + y
                        x = func(4, y = 6)
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Int, 10);
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ThrowsErrorIfKeywordArgumentBeforePositionalArgument()
        {
            var input = """
                        def func(x, y = 5):
                            return x + y
                        x = func(x = 6, 4)
                        """;
            Assert.Throws<InterpreterKeywordArgBeforePositionalArgException>(() => Runner.Run(input));
        }
        
        [Test]
        public void ThrowsErrorIfKeywordAndPositionalArgumentAddressSameParameter()
        {
            var input = """
                        def func(x, y = 5):
                            return x + y
                        x = func(4, x = 6)
                        """;
            Assert.Throws<InterpreterMultipleArgumentsForParameterException>(() => Runner.Run(input));
        }
        
        [Test]
        public void ThrowsErrorIfExtraPositionalArgument()
        {
            var input = """
                        def func(x, y = 5):
                            return x + y
                        x = func(4, 5, 6)
                        """;
            Assert.Throws<Exception>(() => Runner.Run(input));
        }
        
        [Test]
        public void ThrowsErrorIfExtraKeywordArgument()
        {
            var input = """
                        def func(x, y = 5):
                            return x + y
                        x = func(x = 4, y = 5, z = 6)
                        """;
            Assert.Throws<Exception>(() => Runner.Run(input));
        }
    }
    
    [TestFixture]
    public class LambdaFunctions
    {
        [Test]
        public void AllowsCreationOfLambdaFunctions()
        {
            var input = """
                        x = lambda y: y + 5
                        """;
            var expected = new FunctionVariable(
                null,
                [new VariableInstruction("y")],
                [new ReturnInstruction(new OperationInstruction("+", new VariableInstruction("y"), new NumericInstruction(5)))]);
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
}