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
            if (operation == "/=" && BsTypes.Is(BsTypes.Types.Int, result))
            {
                var objectResult = result as ObjectVariable;
                var doubleResult = (objectResult.Values["__value"] as NumericVariable).Value;
                memory.SetVariable(left, BsTypes.Create(BsTypes.Types.Float, doubleResult));
            } else if (operation == "//=" && BsTypes.Is(BsTypes.Types.Float, result))
            {
                var objectResult = result as ObjectVariable;
                var intResult = (objectResult.Values["__value"] as NumericVariable).Value;
                memory.SetVariable(left, BsTypes.Create(BsTypes.Types.Int, intResult));
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
    
    private static readonly Dictionary<string, string> ReversedOperationToOverrideMap = new()
    {
        {"-", "__rsub__"},
        {"/", "__rtruediv__"},
        {"//", "__rfloordiv__"},
        {"%", "__rmod__"},
        {"**", "__rpow__"},
        {"<", "__rlt__"},
        {"<=", "__rle__"},
        {">", "__rgt__"},
        {">=", "__rge__"},
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
            return ConductBooleanOperation(memory, operation, left, right);
        }
        else if (left is ObjectVariable || right is ObjectVariable)
        {
            return ConductObjectOperation(memory, operation, left, right, originalInstruction);
        }
        else if (left is StringVariable || right is StringVariable)
        {
            return ConductStringOperation(memory, operation, left, right);
        }
        else if (left is NumericVariable leftNumeric && right is NumericVariable rightNumeric)
        {
            return ConductBinaryNumericOperation(operation, leftNumeric, rightNumeric);
        } else if (left is null && right is NumericVariable rightUnaryNumeric)
        {
            return ConductUnaryNumericOperation(operation, rightUnaryNumeric);
        }
        else if (left is SequenceVariable leftSequence && right is SequenceVariable rightSequence)
        {
            return ConductBinarySequenceOperation(operation, leftSequence, rightSequence);
        }
        else if (left is SequenceVariable leftSequence2 && right is NumericVariable rightNumeric2)
        {
            return ConductNumericSequenceOperation(operation, leftSequence2, rightNumeric2);
        }
        else if (left is NumericVariable leftNumeric2 && right is SequenceVariable rightSequence2)
        {
            return ConductNumericSequenceOperation(operation, rightSequence2, leftNumeric2);
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
        }
    }
    
    private static Variable ConductBinarySequenceOperation(string operation, SequenceVariable leftSequence, SequenceVariable rightSequence)
    {
        switch (operation)
        {
            case "+":
                return new SequenceVariable(leftSequence.Values.Concat(rightSequence.Values).ToList());
            case "==":
                return CompareSequences(leftSequence, rightSequence) ? new NumericVariable(1) : new NumericVariable(0);
            default:
                throw new InterpreterInvalidOperationException(operation, leftSequence, rightSequence);
        }
        
        bool CompareSequences(SequenceVariable sequence1, SequenceVariable sequence2)
        {
            if (sequence1.Values.Count != sequence2.Values.Count)
            {
                return false;
            }
        
            for (var i = 0; i < sequence1.Values.Count; i++)
            {
                if (!sequence1.Values[i].Equals(sequence2.Values[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    private static Variable ConductNumericSequenceOperation(string operation, SequenceVariable sequence, NumericVariable numeric)
    {
        switch (operation)
        {
            case "*":
                return MultiplySequence();
            case "==":
                return new NumericVariable(0);
            default:
                throw new InterpreterInvalidOperationException(operation, sequence, numeric);
        }
        
        Variable MultiplySequence()
        {
            var values = new List<Variable>();
            for (var i = 0; i < numeric.Value; i++)
            {
                foreach (var value in sequence.Values)
                {
                    values.Add(value);
                }
            }
            return new SequenceVariable(values);
        }
    }
        
        
    private static Variable ConductBooleanOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        // The operators handled here will be the same regardless of type or have complex type interactions
        switch (operation)
        {
            case "or":
                return BsTypes.Create(BsTypes.Types.Bool, GetOrValue());
            case "and":
                return BsTypes.Create(BsTypes.Types.Bool, GetAndValue());
            case "not":
                var rightNot = Truthiness.IsTruthy(memory, right!);
                return BsTypes.Create(BsTypes.Types.Bool, !rightNot);
            case "is":
                return BsTypes.Create(BsTypes.Types.Bool, ReferenceEquals(left, right));
            case "is not":
                return BsTypes.Create(BsTypes.Types.Bool, !ReferenceEquals(left, right));
            case "in":
                return BsTypes.Create(BsTypes.Types.Bool, GetInValue());
            case "not in":
                return BsTypes.Create(BsTypes.Types.Bool, !GetInValue());
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
            if (BsTypes.Is(BsTypes.Types.String, left) && BsTypes.Is(BsTypes.Types.String, right))
            {
                var leftString = BsTypes.GetStringValue(left);
                var rightString = BsTypes.GetStringValue(right);
                return rightString.Contains(leftString);
            } else if (BsTypes.Is(BsTypes.Types.List, right))
            {
                var listValue = BsTypes.GetListValue(right);
                return listValue.Values.Any(x => x.Equals(left));
            } else if (BsTypes.Is(BsTypes.Types.Dictionary, right) && BsTypes.Is(BsTypes.Types.Int, left))
            {
                var dictValue1 = BsTypes.GetDictValue(right);
                var intValue = BsTypes.GetIntValue(left);
                return dictValue1.IntValues.Any(x => x.Key.Equals(intValue));
            } else if (BsTypes.Is(BsTypes.Types.Dictionary, right) && BsTypes.Is(BsTypes.Types.String, left))
            {
                var dictValue2 = BsTypes.GetDictValue(right);
                var stringValue = BsTypes.GetStringValue(left);
                return dictValue2.StringValues.Any(x => x.Key.Equals(stringValue));
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }
    }
        
    private static Variable ConductObjectOperation(Memory memory, string operation, Variable? left, Variable? right, Instruction? originalInstruction = null)
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
                return rightOverride.RunFunction(memory, new ArgumentSet([left, right]), originalInstruction);
            }
        }
        else
        {
            throw new InterpreterInvalidOperationException(operation, left, right);
            
        }

        (FunctionVariable? Left, FunctionVariable? Right) GetOverride()
        {
            FunctionVariable? leftFunc = null;
            if (left is ObjectVariable leftObject)
            {
                string overrideName;
                if (right is null)
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
                
                leftFunc = leftObject.GetMember(memory, new MemberInstruction(overrideName)) as FunctionVariable;
            }
        
            FunctionVariable? rightFunc = null;
            if (right is ObjectVariable rightObject)
            {
                string overrideName;
                if (left is null)
                {
                    UnaryOperationToOverrideMap.TryGetValue(operation, out overrideName);
                }
                else if (ReversedOperationToOverrideMap.Keys.Contains(operation))
                {
                    ReversedOperationToOverrideMap.TryGetValue(operation, out overrideName);
                } 
                else
                {
                    OperationToOverrideMap.TryGetValue(operation, out overrideName);
                }
                
                if (overrideName is null)
                {
                    throw new InterpreterInvalidOperationException(operation, left, right);
                }
                
                rightFunc = rightObject.GetMember(memory, new MemberInstruction(overrideName)) as FunctionVariable;
            }
            
            return (leftFunc, rightFunc);
        }
    }
        
    private static Variable ConductStringOperation(Memory memory, string operation, Variable? left, Variable? right)
    {
        switch (operation)
        {
            case "+":
                var leftString = ConvertToString(left);
                var rightString = ConvertToString(right);
                return new StringVariable(leftString + rightString);
            case "==":
                if (left is StringVariable leftEqualsString && right is StringVariable rightEqualsString)
                {
                    return new NumericVariable(leftEqualsString.Value == rightEqualsString.Value ? 1 : 0);
                }
                else
                {
                    throw new InterpreterInvalidOperationException(operation, left, right);
                }
            case "!=":
                if (left is StringVariable leftNotEqualsString && right is StringVariable rightNotEqualsString)
                {
                    return new NumericVariable(leftNotEqualsString.Value != rightNotEqualsString.Value ? 1 : 0);
                }
                else
                {
                    throw new InterpreterInvalidOperationException(operation, left, right);
                }
            case "*":
                if (left is StringVariable leftStringVariable && right is NumericVariable rightNumericVariable)
                {
                    return MultiplyString(leftStringVariable, rightNumericVariable);
                }
                else if (left is NumericVariable leftNumericVariable &&
                         right is StringVariable rightStringVariable)
                {
                    return MultiplyString(rightStringVariable, leftNumericVariable);
                }
                else
                {
                    throw new InterpreterInvalidOperationException(operation, left, right);
                }
            default:
                throw new InterpreterInvalidOperationException(operation, left, right);
        }
        
        
        
        string ConvertToString(Variable variable)
        {
            if (variable is StringVariable stringVariable)
            {
                return stringVariable.Value;
            }
            else if (variable is NumericVariable numericVariable)
            {
                return numericVariable.Value.ToString();
            }
            else
            {
                throw new InterpreterInvalidOperationException(operation, left, right);
            }
        }

        Variable MultiplyString(StringVariable stringVariable, NumericVariable numericVariable)
        {
            var stringValue = stringVariable.Value;
            var numericValue = numericVariable.Value;
            var result = "";
            for (var i = 0; i < numericValue; i++)
            {
                result += stringValue;
            }
            return new StringVariable(result);
        }
    }
        
    private static Variable ConductBinaryNumericOperation(string operation, NumericVariable leftNumeric, NumericVariable rightNumeric)
    {
        switch (operation)
        {
            case "**":
                return new NumericVariable(Math.Pow(leftNumeric.Value, rightNumeric.Value));
            case "*":
                return new NumericVariable(leftNumeric.Value * rightNumeric.Value);
            case "/":
                return new NumericVariable((double)leftNumeric.Value / (double)rightNumeric.Value);
            case "//":
                return new NumericVariable(Math.Floor((double)leftNumeric.Value / (double)rightNumeric.Value));
            case "%":
                return new NumericVariable(leftNumeric.Value % rightNumeric.Value);
            case "+":
                return new NumericVariable(leftNumeric.Value + rightNumeric.Value);
            case "-":
                return new NumericVariable(leftNumeric.Value - rightNumeric.Value);
            case "==":
                return new NumericVariable(Math.Abs(leftNumeric.Value - rightNumeric.Value) <
                                           Consts.FloatingPointTolerance
                    ? 1
                    : 0);
            case "!=":
                return new NumericVariable(Math.Abs(leftNumeric.Value - rightNumeric.Value) >
                                           Consts.FloatingPointTolerance
                    ? 1
                    : 0);
            case ">":
                var gValue = leftNumeric.Value > rightNumeric.Value;
                return new NumericVariable(gValue ? 1 : 0);
            case ">=":
                var geValue = leftNumeric.Value >= rightNumeric.Value;
                return new NumericVariable(geValue ? 1 : 0);
            case "<":
                var lValue = leftNumeric.Value < rightNumeric.Value;
                return new NumericVariable(lValue ? 1 : 0);
            case "<=":
                var leValue = leftNumeric.Value <= rightNumeric.Value;
                return new NumericVariable(leValue ? 1 : 0);
            case "+=":
                leftNumeric.Value += rightNumeric.Value;
                return new ConstantVariable();
            case "-=":
                leftNumeric.Value -= rightNumeric.Value;
                return new ConstantVariable();
            case "*=":
                leftNumeric.Value *= rightNumeric.Value;
                return new ConstantVariable();
            case "/=":
                leftNumeric.Value /= (double)rightNumeric.Value;
                return new ConstantVariable();
            case "//=":
                leftNumeric.Value = Math.Floor((double)leftNumeric.Value / (double)rightNumeric.Value);
                return new ConstantVariable();
            case "%=":
                leftNumeric.Value %= rightNumeric.Value;
                return new ConstantVariable();
            case "**=":
                leftNumeric.Value = Math.Pow(leftNumeric.Value, rightNumeric.Value);
                return new ConstantVariable();
            default:
                throw new InterpreterInvalidOperationException(operation, leftNumeric, rightNumeric);
        }
    }
    
    private static Variable ConductUnaryNumericOperation(string operation, NumericVariable numeric)
    {
        switch (operation)
        {
            case "-":
                return new NumericVariable(-numeric.Value);
            case "+":
                return new NumericVariable(numeric.Value);
            default:
                throw new InterpreterInvalidOperationException(operation, null, numeric);
        }
    }
}