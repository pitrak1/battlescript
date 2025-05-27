namespace Battlescript;

public static class InterpreterUtilities
{
    public static Variable ConductOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (right is NumberVariable) return ConductNumericalOperation(operation, left, right);
        else if (right is BooleanVariable) return ConductLogicalOperation(operation, left, right);
        else if (right is ObjectVariable) return ConductObjectOperation(memory, operation, left, right);
        else throw new InterpreterInvalidOperationException(operation, left, right);
    }

    public static Variable ConductAssignment(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (operation == "=") return right ?? new NullVariable();

        var operationWithEqualsRemoved = operation.Substring(0, operation.Length - 1);
        return ConductOperation(memory, operationWithEqualsRemoved, left, right);
    }

    private static Variable ConductNumericalOperation(string operation, Variable? left, Variable? right)
    {
        if (left is NumberVariable leftNumber && right is NumberVariable rightNumber)
        {
            switch (operation)
            {
                case "**":
                    return new NumberVariable(Math.Pow((int)leftNumber.Value, (int)rightNumber.Value));
                case "*":
                    return new NumberVariable(leftNumber.Value * rightNumber.Value);
                case "/":
                    return new NumberVariable(leftNumber.Value / rightNumber.Value);
                case "//":
                    return new NumberVariable((int)Math.Floor(leftNumber.Value / rightNumber.Value));
                case "%":
                    return new NumberVariable(leftNumber.Value % rightNumber.Value);
                case "+":
                    return new NumberVariable(leftNumber.Value + rightNumber.Value);
                case "-":
                    return new NumberVariable(leftNumber.Value - rightNumber.Value);
                case "<<":
                    return new NumberVariable((int)leftNumber.Value << (int)rightNumber.Value);
                case ">>":
                    return new NumberVariable((int)leftNumber.Value >> (int)rightNumber.Value);
                case "&":
                    return new NumberVariable((int)leftNumber.Value & (int)rightNumber.Value);
                case "^":
                    return new NumberVariable((int)leftNumber.Value ^ (int)rightNumber.Value);
                case "|":
                    return new NumberVariable((int)leftNumber.Value | (int)rightNumber.Value);
                case "==":
                    return new BooleanVariable(leftNumber.Value == rightNumber.Value);
                case "!=":
                    return new BooleanVariable(leftNumber.Value != rightNumber.Value);
                case ">":
                    return new BooleanVariable(leftNumber.Value > rightNumber.Value);
                case ">=":
                    return new BooleanVariable(leftNumber.Value >= rightNumber.Value);
                case "<":
                    return new BooleanVariable(leftNumber.Value < rightNumber.Value);
                case "<=":
                    return new BooleanVariable(leftNumber.Value <= rightNumber.Value);
                default:
                    throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
        else
        {
            var rightNumber2 = right as NumberVariable;
            switch (operation)
            {
                case "~":
                    return new NumberVariable(~(int)rightNumber2.Value);
                default:
                    throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
    }

    private static Variable ConductLogicalOperation(string operation, Variable? left, Variable? right)
    {
        if (left is BooleanVariable leftBoolean && right is BooleanVariable rightBoolean)
        {
            switch (operation)
            {
                case "and":
                    return new BooleanVariable(leftBoolean.Value && rightBoolean.Value);
                case "or":
                    return new BooleanVariable(leftBoolean.Value || rightBoolean.Value);
                default:
                    throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
        else
        {
            var rightBoolean2 = right as BooleanVariable;
            switch (operation)
            {
                case "not":
                    return new BooleanVariable(!rightBoolean2.Value);
                default:
                    throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
    }

    private static Variable ConductObjectOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (left is ObjectVariable leftObject && right is ObjectVariable rightObject)
        {
            Variable? method;
            switch (operation)
            {
                case "+":
                    method = left.GetItem(memory, "__add__");
                    if (method is FunctionVariable functionVariable)
                    {
                        return functionVariable.RunFunction(memory, [left, right]);
                    }
                    else
                    {
                        throw new InterpreterInvalidOperationException(operation, left, right);
                    }
                default:
                    throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }

    public static bool IsVariableTruthy(Variable variable)
    {
        return variable is BooleanVariable { Value: true };
    }
}