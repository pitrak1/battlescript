using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class InterpreterUtilitiesTests
{
    [TestFixture]
    public class ConductOperation
    {
        [Test]
        public void ReturnsIntegerIfBothOperandsAreIntegers()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "+", 
                new IntegerVariable(5), 
                new IntegerVariable(6));
            Assert.That(result, Is.EqualTo(new IntegerVariable(11)));
        }
        
        [Test]
        public void ReturnsFloatIfEitherOperandIsFloat()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "+", 
                new IntegerVariable(5), 
                new FloatVariable(6.0));
            Assert.That(result, Is.EqualTo(new FloatVariable(11.0)));
        }
        
        [Test]
        public void ReturnsFloatForTrueDivision()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "/", 
                new IntegerVariable(5), 
                new IntegerVariable(2));
            Assert.That(result, Is.EqualTo(new FloatVariable(2.5)));
        }
        
        [Test]
        public void ReturnsIntegerForFloorDivision()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "//", 
                new FloatVariable(10.1), 
                new FloatVariable(2.5));
            Assert.That(result, Is.EqualTo(new IntegerVariable(4)));
        }

        [Test]
        public void HandlesUnaryMathematicalOperators()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "-", 
                null, 
                new FloatVariable(2.5));
            Assert.That(result, Is.EqualTo(new FloatVariable(-2.5)));
        }
        
        [Test]
        public void HandlesBinaryLogicalOperations()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "and", 
                new BooleanVariable(true), 
                new BooleanVariable(false));
            Assert.That(result, Is.EqualTo(new BooleanVariable(false)));
        }
        
        [Test]
        public void HandlesUnaryLogicalOperations()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "not", 
                null, 
                new BooleanVariable(false));
            Assert.That(result, Is.EqualTo(new BooleanVariable(true)));
        }

        [Test]
        public void HandlesObjectOperationsIfOverrideIsPresent()
        {
            // In this case, we're just creating a class that has + overridden with a function that just returns 5
            var memory = new Memory();
            var addFunction = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("other")], 
                [new ReturnInstruction(new IntegerInstruction(5))]);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new IntegerVariable(7)},
                {"__add__", addFunction}
            });
            var objectVariable = classVariable.CreateObject();
            memory.SetVariable(new VariableInstruction("asdf"), objectVariable);
            
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "+", 
                objectVariable, 
                objectVariable);
            Assert.That(result, Is.EqualTo(new IntegerVariable(5)));
        }
    }

    [TestFixture]
    public class ConductAssignment
    {
        [Test]
        public void ReturnsRightIfStandardAssignmentOperator()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductAssignment(memory, "=", null, new IntegerVariable(5));
            Assert.That(result, Is.EqualTo(new IntegerVariable(5)));
        }
        
        [Test]
        public void ConductsOperationOfTruncatedOperatorIfNotStandardAssignmentOperator()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductAssignment(
                memory, 
                "+=", 
                new IntegerVariable(8), 
                new IntegerVariable(5));
            Assert.That(result, Is.EqualTo(new IntegerVariable(13)));
        }
    }
}