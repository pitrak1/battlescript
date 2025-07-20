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
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6));
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 11));
        }
        
        [Test]
        public void ReturnsFloatIfEitherOperandIsFloat()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "+", 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "float", 6.0));
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "float", 11.0));
        }
        
        [Test]
        public void ReturnsFloatForTrueDivision()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "/", 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2));
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "float", 2.5));
        }
        
        [Test]
        public void ReturnsIntegerForFloorDivision()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "//", 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "float", 10.1), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "float", 2.5));
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4));
        }

        [Test]
        public void HandlesUnaryMathematicalOperators()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "-", 
                null, 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5));
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", -5));
        }
        
        [Test]
        public void HandlesBinaryLogicalOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "and", 
                new ConstantVariable(true), 
                new ConstantVariable(false));
            Assertions.AssertVariablesEqual(result, new ConstantVariable(false));
        }
        
        [Test]
        public void HandlesUnaryLogicalOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.StandardOperation(
                memory, 
                "not", 
                null, 
                new ConstantVariable(false));
            Assertions.AssertVariablesEqual(result, new ConstantVariable(true));
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
                {"x", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 7)},
                {"__add__", addFunction}
            });
            var objectVariable = classVariable.CreateObject();
            memory.SetVariable(new VariableInstruction("asdf"), objectVariable);
            
            var result = Operator.StandardOperation(
                memory, 
                "+", 
                objectVariable, 
                objectVariable);
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5));
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
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8));
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
            Assertions.AssertVariablesEqual(result, BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 13));
        }
    }
}