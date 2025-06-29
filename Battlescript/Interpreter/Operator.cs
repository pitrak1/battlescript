namespace Battlescript;

public static class Operator
{
    public static Variable StandardOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        return ConductOperationWithOverride(memory, operation, operation, left, right);
    }
    
    public static Variable AssignmentOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        // Assignment operations should return the value to be assigned. The calling function is actually responsible
        // for assigning the value to the variable
        if (operation == "=") return right ?? new ConstantVariable();
        
        var operationWithEqualsRemoved = AssignmentOperatorToStandardOperatorMap[operation];
        return ConductOperationWithOverride(memory, operationWithEqualsRemoved, operation, left, right);
    }

    private static readonly Dictionary<string, string> AssignmentOperatorToStandardOperatorMap = new()
    {
        {"+=", "+"},
        {"-=", "-"},
        {"*=", "*"},
        {"/=", "/"},
        {"//=", "//"},
        {"%=", "%"},
        {"**=", "**"},
    };
    
    private static Variable ConductOperationWithOverride(Memory memory, string operation, string originalOperation, Variable? left, Variable? right)
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
            return ConductObjectOperation(memory, operation, originalOperation, left, right);
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
        if (left.Type == Consts.VariableTypes.Value || right.Type == Consts.VariableTypes.Value)
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
        
        return operation == "is" ? new ConstantVariable(left == right) : new ConstantVariable(left != right);
    }

    private static Variable ConductInOperation(string operation, Variable? left, Variable? right)
    {
        if (left is StringVariable leftString && right is StringVariable rightString)
        {
            // if both operands are strings, we search for a substring
            var isContained = rightString.Value.Contains(leftString.Value);
            return operation == "in" ? new ConstantVariable(isContained) : new ConstantVariable(!isContained);
        } else if (right is ListVariable rightList)
        {
            // If the right operand is a list, we search for a matching element
            var found = rightList.Values.Any(x => x.Equals(left));
            return operation == "in" ? new ConstantVariable(found) : new ConstantVariable(!found);
        } else if (right is DictionaryVariable rightDictionary)
        {
            // If the right operand is a dictionary, we search for a matching key
            var found = rightDictionary.Values.Any(x => x.Key.Equals(left));
            return operation == "in" ? new ConstantVariable(found) : new ConstantVariable(!found);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }
    
    private static Variable ConductObjectOperation(Memory memory, string operation, string originalOperation, Variable? left, Variable? right)
    {
        if (left is null)
        {
            return ConductUnaryObjectOperation(memory, operation, originalOperation, right);
        }
        
        OperationToOverrideMap.TryGetValue(originalOperation, out var overrideName);
        
        if (overrideName is null)
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }

        FunctionVariable? leftOverride = null;
        if (left is ObjectVariable leftObject)
        {
            leftOverride = leftObject.GetItem(memory, overrideName) as FunctionVariable;
        }
        
        FunctionVariable? rightOverride = null;
        if (right is ObjectVariable rightObject)
        {
            rightOverride = rightObject.GetItem(memory, overrideName) as FunctionVariable;
        }

        // If left operand is an object and has an override, use that one.  Otherwise, if right operand is an object
        // and has an override, use that one.
        if (leftOverride is not null)
        {
            return leftOverride.RunFunction(memory, [left!, right ?? new ConstantVariable()]);
        } else if (rightOverride is not null)
        {
            return rightOverride.RunFunction(memory, [right!, left ?? new ConstantVariable()]);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }
    
    private static readonly Dictionary<string, string> OperationToOverrideMap = new()
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
        {">=", "__ge__"},
        {"+=", "__iadd__"},
        {"-=", "__isub__"},
        {"*=", "__imul__"},
        {"/=", "__itruediv__"},
        {"//=", "__ifloordiv__"},
        {"%=", "__imod__"},
        {"**=", "__ipow__"},
    };
    
    private static Variable ConductUnaryObjectOperation(Memory memory, string operation, string originalOperation, Variable? right)
    {
        UnaryOperationToOverrideMap.TryGetValue(originalOperation, out var overrideName);
        
        if (overrideName is null)
        {
            throw new InterpreterInvalidOperationException(operation, null, right);
        }
        
        FunctionVariable? rightOverride = null;
        if (right is ObjectVariable rightObject)
        {
            rightOverride = rightObject.GetItem(memory, overrideName) as FunctionVariable;
        }
        
        var rightObjectVariable = right as ObjectVariable;
        if (rightOverride is not null)
        {
            return rightOverride.RunFunction(memory, new List<Instruction>(), rightObjectVariable);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, null, right);
        }
    }
    
    private static readonly Dictionary<string, string> UnaryOperationToOverrideMap = new()
    {
        {"+", "__pos__"},
        {"-", "__neg__"},
    };
    
    private static Variable ConductNumericalOperation(string operation, Variable? left, Variable? right)
    {
        // This may be made easier by using a generic __numeric__ variable type with a dynamic value instead of this
        // crazy bs
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
        if (left is ConstantVariable leftBoolean && right is ConstantVariable rightBoolean)
        {
            if (operation == "and")
            {
                return new ConstantVariable(Truthiness.IsTruthy(leftBoolean) && Truthiness.IsTruthy(rightBoolean));
            } else if (operation == "or")
            {
                return new ConstantVariable(Truthiness.IsTruthy(leftBoolean) || Truthiness.IsTruthy(rightBoolean));
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        } else if (right is ConstantVariable rightBoolean2 && operation == "not")
        {
            return new ConstantVariable(!Truthiness.IsTruthy(rightBoolean2));
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
                return new ConstantVariable((bool)result);
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
                return new ConstantVariable((bool)result);
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
}