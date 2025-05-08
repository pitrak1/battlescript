namespace Battlescript;

public static class InterpreterUtilities
{
    public static Variable ConductOperation(string operation, Variable left, Variable right)
    {
        if (right is NumberVariable rightNumber)
        {
            if (left is NumberVariable leftNumber)
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
                        throw new Exception("Invalid operation: " + operation);
                }
            }
            else
            {
                switch (operation)
                {
                    case "~":
                        return new NumberVariable(~(int)rightNumber.Value);
                    default:
                        throw new Exception("Invalid operation: " + operation);
                }
            }
        } else if (right is BooleanVariable rightBoolean)
        {
            if (left is BooleanVariable leftBoolean)
            {
                switch (operation)
                {
                    case "and":
                        return new BooleanVariable(leftBoolean.Value && rightBoolean.Value);
                    case "or":
                        return new BooleanVariable(leftBoolean.Value || rightBoolean.Value);
                    default:
                        throw new Exception("Invalid operation: " + operation);
                }
            }
            else
            {
                switch (operation)
                {
                    case "not":
                        return new BooleanVariable(!rightBoolean.Value);
                    default:
                        throw new Exception("Invalid operation: " + operation);
                }
            }
        }
        else
        {
            throw new Exception("Invalid operation: " + operation);
        }
    }

    public static Variable ConductAssignment(string operation, Variable left, Variable right)
    {
        if (operation == "=")
        {
            return right;
        }
        else
        {
            return ConductOperation(operation.Substring(0, operation.Length - 1), left, right);
        }
    }

    public static bool IsVariableTruthy(Variable variable)
    {
        return variable is BooleanVariable { Value: true };
    }
}