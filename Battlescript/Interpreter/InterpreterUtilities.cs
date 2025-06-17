namespace Battlescript;

public static class InterpreterUtilities
{
    public static Variable ConductOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (operation == "is" || operation == "is not")
        {
            return ConductIsOperation(operation, left, right);
        }
        else if (operation == "in" || operation == "not in")
        {
            return ConductInOperation(operation, left, right);
        }
        else if (left is ObjectVariable || right is ObjectVariable)
        {
            return ConductObjectOperation(memory, operation, left, right);
        }
        else if (Consts.NumericalOperators.Contains(operation))
        {
            return ConductNumericalOperation(operation, left, right);
        } 
        else
        {
            return ConductBooleanOperation(operation, left, right);
        }
    }

    private static Variable ConductIsOperation(string operation, Variable? left, Variable? right)
    {
        if (left is not ReferenceVariable || right is not ReferenceVariable)
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }

        return operation == "is" ? new BooleanVariable(left == right) : new BooleanVariable(left != right);
    }

    private static Variable ConductInOperation(string operation, Variable? left, Variable? right)
    {
        if (left is StringVariable leftString && right is StringVariable rightString)
        {
            var isContained = rightString.Value.Contains(leftString.Value);
            return operation == "in" ? new BooleanVariable(isContained) : new BooleanVariable(!isContained);
        } else if (right is ListVariable rightList)
        {
            var found = rightList.Values.Any(x => x.Equals(left));
            return operation == "in" ? new BooleanVariable(found) : new BooleanVariable(!found);
        } else if (right is DictionaryVariable rightDictionary)
        {
            var found = rightDictionary.Values.Any(x => x.Left is not null && x.Left.Equals(left));
            return operation == "in" ? new BooleanVariable(found) : new BooleanVariable(!found);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }

    public static Variable ConductAssignment(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (operation == "=") return right ?? new NoneVariable();
        var operationWithEqualsRemoved = AssignmentOperatorToStandardOperatorMap[operation];
        return ConductOperation(memory, operationWithEqualsRemoved, left, right);
    }

    private static Variable ConductNumericalOperation(string operation, Variable? left, Variable? right)
    {
        if (left is null && right is not null)
        {
            return ConductUnaryNumericalOperation(operation, right);
        }
        
        double leftDouble;
        double rightDouble;
        try
        {
            leftDouble = GetValueAsDouble(left);
            rightDouble = GetValueAsDouble(right);
        }
        catch (InterpreterInvalidOperationException)
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
        
        switch (operation)
        {
            case "**":
                return CreateResultWithType(Math.Pow(leftDouble, rightDouble), operation, left, right);
            case "*":
                return CreateResultWithType(leftDouble * rightDouble, operation, left, right);
            case "/":
                return CreateResultWithType(leftDouble / rightDouble, operation, left, right);
            case "//":
                return CreateResultWithType(Math.Floor(leftDouble / rightDouble), operation, left, right);
            case "%":
                return CreateResultWithType(leftDouble % rightDouble, operation, left, right);
            case "+":
                return CreateResultWithType(leftDouble + rightDouble, operation, left, right);
            case "-":
                return CreateResultWithType(leftDouble - rightDouble, operation, left, right);
            case "==":
                return CreateResultWithType(Math.Abs(leftDouble - rightDouble) < Consts.FloatingPointTolerance, operation, left, right);
            case "!=":
                return CreateResultWithType(Math.Abs(leftDouble - rightDouble) > Consts.FloatingPointTolerance, operation, left, right);
            case ">":
                return CreateResultWithType(leftDouble > rightDouble, operation, left, right);
            case ">=":
                return CreateResultWithType(leftDouble >= rightDouble, operation, left, right);
            case "<":
                return CreateResultWithType(leftDouble < rightDouble, operation, left, right);
            case "<=":
                return CreateResultWithType(leftDouble <= rightDouble, operation, left, right);
            default:
                throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }

    private static Variable ConductUnaryNumericalOperation(string operation, Variable right)
    {
        switch (operation)
        {
            case "+":
                return right;
            case "-":
                if (right is IntegerVariable rightInt)
                {
                    return new IntegerVariable(-rightInt.Value);
                } 
                else if (right is FloatVariable rightFloat)
                {
                    return new FloatVariable(-rightFloat.Value);
                }
                else
                {
                    throw new InterpreterInvalidOperationException(operation, null, right);
                }
            default:
                throw new InterpreterInvalidOperationException(operation, null, right);
        }
    }

    private static Variable ConductBooleanOperation(string operation, Variable? left, Variable? right)
    {
        if (left is BooleanVariable leftBoolean && right is BooleanVariable rightBoolean)
        {
            if (operation == "and")
            {
                return new BooleanVariable(leftBoolean.Value && rightBoolean.Value);
            } else if (operation == "or")
            {
                return new BooleanVariable(leftBoolean.Value || rightBoolean.Value);
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        } else if (right is BooleanVariable rightBoolean2 && operation == "not")
        {
            return new BooleanVariable(!rightBoolean2.Value);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }

    private static double GetValueAsDouble(Variable? variable)
    {
        if (variable is IntegerVariable integerVariable)
        {
            return integerVariable.Value;
        } else if (variable is FloatVariable floatVariable)
        {
            return floatVariable.Value;
        }
        else
        {
            throw new InterpreterInvalidOperationException("", variable, variable);
        }
    }
    
    private static Variable CreateResultWithType(dynamic result, string operation, Variable? left, Variable? right)
    {
        if (operation == "/")
        {
            return new FloatVariable(result);
        }
        else if (operation == "//")
        {
            return new IntegerVariable((int)result);
        }
        else if (left is IntegerVariable && right is IntegerVariable)
        {
            if (result is int or double)
            {
                return new IntegerVariable((int)result);
            }
            else if (result is bool)
            {
                return new BooleanVariable((bool)result);
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        } 
        else if (left is IntegerVariable or FloatVariable && right is IntegerVariable or FloatVariable)
        {
            if (result is int or double)
            {
                return new FloatVariable(result);
            }
            else if (result is bool)
            {
                return new BooleanVariable((bool)result);
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        } 
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }

    private static Variable ConductObjectOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (left is ObjectVariable leftObject)
        {
            var leftOverrideMethod = leftObject.GetOverride(memory, BinaryOperationToOverrideMap[operation]);
            if (leftOverrideMethod is not null)
            {
                return leftOverrideMethod.RunFunction(memory, [left, right ?? new NoneVariable()]);
            }
            else if (right is ObjectVariable rightObject1)
            {
                var rightOverrideMethod = rightObject1.GetOverride(memory, BinaryOperationToOverrideMap[operation]);
                if (rightOverrideMethod is not null)
                {
                    return rightOverrideMethod.RunFunction(memory, [right, left]);
                }
                else
                {
                    throw new InterpreterInvalidOperationException(operation, left, right);
                }
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
        else if (right is ObjectVariable rightObject2)
        {
            var rightOverrideMethod = rightObject2.GetOverride(memory, BinaryOperationToOverrideMap[operation]);
            if (rightOverrideMethod is not null)
            {
                return rightOverrideMethod.RunFunction(memory, [right, left ?? new NoneVariable()]);
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }

    private static readonly Dictionary<string, string> BinaryOperationToOverrideMap = new Dictionary<string, string>()
        {
            {"+", "__add__"},
            {"-", "__sub__"},
            {"*", "__mul__"},
            {"/", "__truediv__"},
            {"//", "__floordiv__"},
            {"%", "__mod__"},
            {"**", "__pow__"},
            {"==", "__eq__"},
            {"!=", "__ne__"},
            {"<", "__lt__"},
            {"<=", "__le__"},
            {">", "__gt__"},
            {">=", "__ge__"}
        };

    private static readonly Dictionary<string, string> AssignmentOperatorToStandardOperatorMap =
        new Dictionary<string, string>()
        {
            {"+=", "+"},
            {"-=", "-"},
            {"*=", "*"},
            {"/=", "/"},
            {"//=", "//"},
            {"%=", "%"},
            {"**=", "**"},
        };

    public static bool IsVariableTruthy(Variable variable)
    {
        switch (variable)
        {
            case BooleanVariable booleanVariable:
                return booleanVariable.Value;
            case IntegerVariable integerVariable:
                return integerVariable.Value != 0;
            case FloatVariable floatVariable:
                return floatVariable.Value != 0;
            case StringVariable stringVariable:
                return stringVariable.Value.Length > 0;
            case ListVariable listVariable:
                return listVariable.Values.Count > 0;
            case NoneVariable:
                return false;
            case ClassVariable:
                return true;
            case ObjectVariable:
                return true;
            case DictionaryVariable dictionaryVariable:
                return dictionaryVariable.Values.Count > 0;
            case FunctionVariable:
                return true;
            case KeyValuePairVariable:
                return true;
            default:
                throw new Exception("Won't get here");
        }
    }
}