using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class OperatorTests
{
    [TestFixture]
    public class StandardOperation
    {
        [Test]
        public void ReturnsIntegerIfBothOperandsAreIntegers()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "+", 
                BsTypes.Create(memory, BsTypes.Types.Int, 5), 
                BsTypes.Create(memory, BsTypes.Types.Int, 6));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 11));
        }
        
        [Test]
        public void ReturnsFloatIfEitherOperandIsFloat()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "+", 
                BsTypes.Create(memory, BsTypes.Types.Int, 5), 
                BsTypes.Create(memory, BsTypes.Types.Float, 6.0));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Float, 11.0));
        }
        
        [Test]
        public void ReturnsFloatForTrueDivision()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "/", 
                BsTypes.Create(memory, BsTypes.Types.Int, 5), 
                BsTypes.Create(memory, BsTypes.Types.Int, 2));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Float, 2.5));
        }
        
        [Test]
        public void ReturnsIntegerForFloorDivision()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "//", 
                BsTypes.Create(memory, BsTypes.Types.Float, 10.1), 
                BsTypes.Create(memory, BsTypes.Types.Float, 2.5));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 4));
        }

        [Test]
        public void HandlesUnaryMathematicalOperators()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "-", 
                null, 
                BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, -5));
        }
        
        [Test]
        public void HandlesBinaryLogicalOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "and", 
                BsTypes.Create(memory, BsTypes.Types.Bool, true), 
                BsTypes.Create(memory, BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesUnaryLogicalOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "not", 
                null, 
                BsTypes.Create(memory, BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesObjectOperationsIfOverrideIsPresent()
        {
            // In this case, we're just creating a class that has + overridden with a function that just returns 5
            var memory = Runner.Run("");
            var addFunction = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("other")], 
                [new ReturnInstruction(new NumericInstruction(5))]);
            var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                {"x", BsTypes.Create(memory, BsTypes.Types.Int, 7)},
                {"__add__", addFunction}
            });
            var objectVariable = classVariable.CreateObject();
            memory.SetVariable(new VariableInstruction("asdf"), objectVariable);
            
            var result = Operator.StandardOperation(
                memory, 
                "+", 
                objectVariable, 
                objectVariable);
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 5));
        }
    }

    [TestFixture]
    public class AssignmentOperation
    {
        [Test]
        public void ReturnsRightIfStandardAssignmentOperator()
        {
            var memory = Runner.Run("");
            var result = Operator.AssignmentOperation(memory, "=", null, new NumericInstruction(8));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 8));
        }
        
        [Test]
        public void ConductsOperationOfTruncatedOperatorIfNotStandardAssignmentOperator()
        {
            var memory = Runner.Run("");
            var result = Operator.AssignmentOperation(
                memory, 
                "+=", 
                new NumericInstruction(8), 
                new NumericInstruction(5));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 13));
        }
    }
}