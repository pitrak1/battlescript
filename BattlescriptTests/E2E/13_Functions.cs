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
            var expected = new Variable(
                Consts.VariableTypes.Function, 
                new List<Variable>(),
                new List<Instruction>()
                {
                    new (
                        Consts.InstructionTypes.Assignment, 
                        "=", 
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "x"),
                        new Instruction(Consts.InstructionTypes.Number, 5))
                });
            E2EAssertions.AssertVariableValueFromInput(input, "func", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithOneArgument()
        {
            var input = "def func(asdf):\n\tx = asdf";
            var expected = new Variable(
                Consts.VariableTypes.Function, 
                new List<string>()
                {
                    "asdf"
                },
                new List<Instruction>()
                {
                    new (
                        Consts.InstructionTypes.Assignment, 
                        "=", 
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "x"),
                        new Instruction(Consts.InstructionTypes.Variable, "asdf"))
                });
            E2EAssertions.AssertVariableValueFromInput(input, "func", expected);
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithMultipleArguments()
        {
            var input = "def func(asdf, qwer):\n\tx = asdf";
            var expected = new Variable(
                Consts.VariableTypes.Function, 
                new List<string>()
                {
                    "asdf",
                    "qwer"
                },
                new List<Instruction>()
                {
                    new (
                        Consts.InstructionTypes.Assignment, 
                        "=", 
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "x"),
                        new Instruction(Consts.InstructionTypes.Variable, "asdf"))
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
            var expected = new Variable(Consts.VariableTypes.Number, 5);
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }
    }
}