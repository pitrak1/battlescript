namespace Battlescript;

public static class Operator
{
    public static void Assign(Memory memory, string operation, VariableInstruction left, Instruction? right, Instruction? originalInstruction = null)
    {
        var rightVariable = right?.Interpret(memory);
        if (operation == "=")
        {
            memory.SetVariable(left, rightVariable!);
        }
        else
        {
            var leftVariable = left.Interpret(memory);
            var result = Operate(memory, operation, leftVariable, rightVariable, originalInstruction);
            if (operation == "/=" && memory.Is(Memory.BsTypes.Int, result))
            {
                var objectResult = result as ObjectVariable;
                var doubleResult = (objectResult.Values["__value"] as NumericVariable).Value;
                memory.SetVariable(left, memory.Create(Memory.BsTypes.Float, doubleResult));
            } else if (operation == "//=" && memory.Is(Memory.BsTypes.Float, result))
            {
                var objectResult = result as ObjectVariable;
                var intResult = (objectResult.Values["__value"] as NumericVariable).Value;
                memory.SetVariable(left, memory.Create(Memory.BsTypes.Int, intResult));
            }
            else
            {
                memory.SetVariable(left, result);
            }
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
    
    public static Variable Operate(Memory memory, string operation, Variable? left, Variable? right, Instruction? originalInstruction = null)
    {
        if (Consts.BooleanOperators.Contains(operation))
        {
            return ConductBooleanOperation();
        }
        else if (left is ObjectVariable || right is ObjectVariable)
        {
            return ConductObjectOperation();
        }
        else
        {
            return ConductStandardOperation();
        }
        
        Variable ConductBooleanOperation()
        {
            // The operators handled here will be the same regardless of type or have complex type interactions
            switch (operation)
            {
                case "or":
                    return memory.Create(Memory.BsTypes.Bool, GetOrValue());
                case "and":
                    return memory.Create(Memory.BsTypes.Bool, GetAndValue());
                case "not":
                    var rightNot = Truthiness.IsTruthy(memory, right!);
                    return memory.Create(Memory.BsTypes.Bool, !rightNot);
                case "is":
                    return memory.Create(Memory.BsTypes.Bool, ReferenceEquals(left, right));
                case "is not":
                    return memory.Create(Memory.BsTypes.Bool, !ReferenceEquals(left, right));
                case "in":
                    return memory.Create(Memory.BsTypes.Bool, GetInValue());
                case "not in":
                    return memory.Create(Memory.BsTypes.Bool, !GetInValue());
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
                if (memory.Is(Memory.BsTypes.String, left) && memory.Is(Memory.BsTypes.String, right))
                {
                    var leftString = memory.GetStringValue(left);
                    var rightString = memory.GetStringValue(right);
                    return rightString.Contains(leftString);
                } else if (memory.Is(Memory.BsTypes.List, right))
                {
                    var listValue = memory.GetListValue(right);
                    return listValue.Values.Any(x => x.Equals(left));
                } else if (memory.Is(Memory.BsTypes.Dictionary, right) && memory.Is(Memory.BsTypes.Int, left))
                {
                    var dictValue1 = memory.GetDictValue(right);
                    var intValue = memory.GetIntValue(left);
                    return dictValue1.IntValues.Any(x => x.Key.Equals(intValue));
                } else if (memory.Is(Memory.BsTypes.Dictionary, right) && memory.Is(Memory.BsTypes.String, left))
                {
                    var dictValue2 = memory.GetDictValue(right);
                    var stringValue = memory.GetStringValue(left);
                    return dictValue2.StringValues.Any(x => x.Key.Equals(stringValue));
                }
                else
                {
                    throw new InterpreterInvalidOperationException(operation, left, right);
                }
            }
        }
        
        Variable ConductObjectOperation()
        {
            var (leftOverride, rightOverride) = GetOverride();

            // If left operand is an object and has an override, use that one.  Otherwise, if right operand is an object
            // and has an override, use that one.
            if (leftOverride is not null)
            {
                if (right is null)
                {
                    return leftOverride.RunFunction(memory, new ArgumentSet([left]), originalInstruction);
                }
                else
                {
                    return leftOverride.RunFunction(memory, new ArgumentSet([left, right]), originalInstruction);
                }
            } else if (rightOverride is not null)
            {
                if (left is null)
                {
                    return rightOverride.RunFunction(memory, new ArgumentSet([right]), originalInstruction);
                }
                else
                {
                    return rightOverride.RunFunction(memory, new ArgumentSet([right, left]), originalInstruction);
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
        
        Variable ConductStandardOperation()
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
}