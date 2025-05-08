using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class FunctionDefinitions
    {
        [Test]
        public void HandlesFunctionDefinitionWithNoArguments()
        {
            var input = "def func():\n\tx = 5";
            var expected = new FunctionVariable(
                new List<Instruction>(),
                new List<Instruction>()
                {
                    new AssignmentInstruction(
                        operation: "=", 
                        left: new VariableInstruction("x"),
                        right: new NumberInstruction(5))
                });
            E2EAssertions.AssertVariableValueFromInput(input, "func", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithOneArgument()
        {
            var input = "def func(asdf):\n\tx = asdf";
            var expected = new FunctionVariable(
                new List<Instruction>()
                {
                    new VariableInstruction("asdf")
                },
                new List<Instruction>()
                {
                    new AssignmentInstruction(
                        operation: "=", 
                        left: new VariableInstruction("x"),
                        right: new VariableInstruction("asdf"))
                });
            E2EAssertions.AssertVariableValueFromInput(input, "func", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithMultipleArguments()
        {
            var input = "def func(asdf, qwer):\n\tx = asdf";
            var expected = new FunctionVariable(
                new List<Instruction>()
                {
                    new VariableInstruction("asdf"),
                    new VariableInstruction("qwer")
                },
                new List<Instruction>()
                {
                    new AssignmentInstruction(
                        operation: "=", 
                        left: new VariableInstruction("x"),
                        right: new VariableInstruction("asdf"))
                });
            E2EAssertions.AssertVariableValueFromInput(input, "func", expected);
        }
    }
    
    [TestFixture]
    public class FunctionCalls
    {
        [Test]
        public void HandlesFunctionCallWithNoArguments()
        {
            var input = "x = 6\ndef func():\n\tx = 5\nfunc()";
            var expected = new NumberVariable(5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
        
        [Test]
        public void HandlesFunctionCallWithReturnValue()
        {
            var input = "def func():\n\treturn 6\nx = func()";
            var expected = new NumberVariable(6);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }

        [Test]
        public void HandlesFunctionCallWithArguments()
        {
            var input = "def func(x, y):\n\treturn x + y\nx = func(2, 3)";
            var expected = new NumberVariable(5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}