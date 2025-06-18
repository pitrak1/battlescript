using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public class FunctionsTests
{
    [TestFixture]
    public class BasicFunctionDefinitions
    {
        [Test]
        public void HandlesFunctionDefinitionWithNoArguments()
        {
            var input = @"
def func():
    x = 5";
            var expected = new FunctionVariable(
                [],
                [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new IntegerInstruction(5))
                ]);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("func"));
            Assert.That(result[0]["func"], Is.EqualTo(expected));
        }

        [Test]
        public void HandlesFunctionDefinitionWithOneArgument()
        {
            var input = @"
def func(asdf):
    x = asdf";
            var expected = new FunctionVariable(
                [new VariableInstruction("asdf")],
                [
                    new AssignmentInstruction(
                        operation: "=",
                        left: new VariableInstruction("x"),
                        right: new VariableInstruction("asdf"))
                ]);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("func"));
            Assert.That(result[0]["func"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFunctionDefinitionWithMultipleArguments()
        {
            var input = @"
def func(asdf, qwer):
    x = asdf";
            var expected = new FunctionVariable(
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
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("func"));
            Assert.That(result[0]["func"], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class BasicFunctionCalls
    {
        [Test]
        public void HandlesFunctionCallWithNoArguments()
        {
            var input = @"
x = 6
def func():
    x = 5
func()";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFunctionCallWithArguments()
        {
            var input = @"
x = 6
def func(y, z):
    x = y + z
func(2, 3)";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ReturnValues
    {
        [Test]
        public void HandlesBasicValues()
        {
            var input = @"
def func():
    return 15
x = func()";
            var expected = new IntegerVariable(15);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesExpressions()
        {
            var input = @"
def func(y, z):
    return y + z
x = func(4, 8)";
            var expected = new IntegerVariable(12);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void StopsExecution()
        {
            var input = @"
x = 4
def func():
    x = 5
    return
    x = 6
func()";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}