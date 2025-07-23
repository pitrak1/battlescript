namespace Battlescript;

public static class Operator
{
    public static Variable Assign(Memory memory, string operation, Instruction? left, Instruction? right)
    {
        // Assignment operations should return the value to be assigned. The calling function is actually responsible
        // for assigning the value to the variable
        var rightVariable = right?.Interpret(memory);
        if (operation == "=")
        {
            return rightVariable ?? new ConstantVariable();
        }
        else
        {
            var leftVariable = left?.Interpret(memory);
            var operationWithEqualsRemoved = AssignmentOperatorToStandardOperatorMap[operation];
            return ConductOperation(memory, operationWithEqualsRemoved, operation, leftVariable, rightVariable);
        }
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
    
    public static Variable Operate(Memory memory, string operation, Variable? left, Variable? right)
    {
        return ConductOperation(memory, operation, operation, left, right);
    }
    
    private static Variable ConductOperation(Memory memory, string operation, string originalOperation, Variable? left, Variable? right)
    {
        if (Consts.CommonOperators.Contains(operation))
        {
            return ConductCommonOperation(memory, operation, left, right);
        }
        else if (left is ObjectVariable || right is ObjectVariable)
        {
            return ConductObjectOperation(memory, originalOperation, left, right);
        }
        else
        {
            return ConductStandardOperation(memory, operation, left, right);
        }
    }

    private static Variable ConductCommonOperation(
        Memory memory, 
        string operation, 
        Variable? left, 
        Variable? right)
    {
        // The operators handled here will be the same regardless of type or have complex type interactions
        switch (operation)
        {
            case "or":
                return BsTypes.Create(memory, BsTypes.Types.Bool, GetOrValue());
            case "and":
                return BsTypes.Create(memory, BsTypes.Types.Bool, GetAndValue());
            case "not":
                var rightNot = Truthiness.IsTruthy(memory, right!);
                return BsTypes.Create(memory, BsTypes.Types.Bool, !rightNot);
            case "is":
                return BsTypes.Create(memory, BsTypes.Types.Bool, ReferenceEquals(left, right));
            case "is not":
                return BsTypes.Create(memory, BsTypes.Types.Bool, !ReferenceEquals(left, right));
            case "in":
                return BsTypes.Create(memory, BsTypes.Types.Bool, GetInValue());
            case "not in":
                return BsTypes.Create(memory, BsTypes.Types.Bool, !GetInValue());
            default:
                throw new Exception("Won't get here");
        }


        bool GetOrValue()
        {
            if (Truthiness.IsTruthy(memory, left!))
            {
                return true;
            }
            else
            {
                return Truthiness.IsTruthy(memory, right!);
            }
        }

        bool GetAndValue()
        {
            if (!Truthiness.IsTruthy(memory, left!))
            {
                return false;
            }
            else
            {
                return Truthiness.IsTruthy(memory, right!);
            }
        }
        
        bool GetInValue()
        {
            if (left is StringVariable leftString && right is StringVariable rightString)
            {
                return rightString.Value.Contains(leftString.Value);
            } else if (BsTypes.Is(memory, BsTypes.Types.List, right))
            {
                var listValue = BsTypes.GetListValue(memory, right);
                return listValue.Values.Any(x => x.Equals(left));
            } else if (right is DictionaryVariable rightDictionary && BsTypes.Is(memory, BsTypes.Types.Int, left))
            {
                var intValue = BsTypes.GetIntValue(memory, left);
                return rightDictionary.IntValues.Any(x => x.Key.Equals(intValue));
            } else if (right is DictionaryVariable rightDictionary2 && left is StringVariable leftString2)
            {
                return rightDictionary2.StringValues.Any(x => x.Key.Equals(leftString2.Value));
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
    }
    
    private static Variable ConductObjectOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        var (leftOverride, rightOverride) = GetOverride();

        // If left operand is an object and has an override, use that one.  Otherwise, if right operand is an object
        // and has an override, use that one.
        if (leftOverride is not null)
        {
            if (right is null)
            {
                return leftOverride.RunFunction(memory, new List<Variable>(), left as ObjectVariable);
            }
            else
            {
                return leftOverride.RunFunction(memory, [right], left as ObjectVariable);
            }
        } else if (rightOverride is not null)
        {
            if (left is null)
            {
                return rightOverride.RunFunction(memory, new List<Variable>(), right as ObjectVariable);
            }
            else
            {
                return rightOverride.RunFunction(memory, [left], right as ObjectVariable);
            }
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
            
        }

        (FunctionVariable? Left, FunctionVariable? Right) GetOverride()
        {
            string overrideName;
            if (left is null || right is null)
            {
                UnaryOperationToOverrideMap.TryGetValue(operation, out overrideName);
            }
            else
            {
                OperationToOverrideMap.TryGetValue(operation, out overrideName);
            }
            
            if (overrideName is null)
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
            
            FunctionVariable? leftFunc = null;
            if (left is ObjectVariable leftObject)
            {
                leftFunc = leftObject.GetMember(memory, new MemberInstruction(overrideName)) as FunctionVariable;
            }
        
            FunctionVariable? rightFunc = null;
            if (right is ObjectVariable rightObject)
            {
                rightFunc = rightObject.GetMember(memory, new MemberInstruction(overrideName)) as FunctionVariable;
            }
            
            return (leftFunc, rightFunc);
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

    private static readonly Dictionary<string, string> UnaryOperationToOverrideMap = new()
    {
        { "+", "__pos__" },
        { "-", "__neg__" },
    };
    
    private static Variable ConductStandardOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        if (left is not null)
        {
            return left.Operate(memory, operation, right);
        } else if (right is not null)
        {
            return right.Operate(memory, operation, left, true);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }
}