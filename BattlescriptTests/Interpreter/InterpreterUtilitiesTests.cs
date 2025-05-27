using Battlescript;

namespace BattlescriptTests;

public class InterpreterUtilitiesTests
{
    [TestFixture]
    public class ConductOperation
    {
        [Test]
        public void HandlesBinaryNumericalOperations()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "+", 
                new NumberVariable(5.0), 
                new NumberVariable(6.0));
            Assert.That(result, Is.EqualTo(new NumberVariable(11.0)));
        }
        
        [Test]
        public void HandlesUnaryNumericalOperations()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "~", 
                null, 
                new NumberVariable(6.0));
            Assert.That(result, Is.EqualTo(new NumberVariable(-7.0)));
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
                [new ReturnInstruction(new NumberInstruction(5.0))]);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new NumberVariable(7.0)},
                {"__add__", addFunction}
            });
            var objectVariable = classVariable.CreateObject();
            memory.SetVariable(new VariableInstruction("asdf"), objectVariable);
            
            var result = InterpreterUtilities.ConductOperation(
                memory, 
                "+", 
                objectVariable, 
                objectVariable);
            Assert.That(result, Is.EqualTo(new NumberVariable(5.0)));
        }
    }

    [TestFixture]
    public class ConductAssignment
    {
        [Test]
        public void ReturnsRightIfStandardAssignmentOperator()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductAssignment(memory, "=", null, new NumberVariable(5.0));
            Assert.That(result, Is.EqualTo(new NumberVariable(5.0)));
        }
        
        [Test]
        public void ConductsOperationOfTruncatedOperatorIfNotStandardAssignmentOperator()
        {
            var memory = new Memory();
            var result = InterpreterUtilities.ConductAssignment(
                memory, 
                "+=", 
                new NumberVariable(8.0), 
                new NumberVariable(5.0));
            Assert.That(result, Is.EqualTo(new NumberVariable(13.0)));
        }
    }
}