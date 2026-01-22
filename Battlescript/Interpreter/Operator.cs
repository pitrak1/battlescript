using System.Collections.Frozen;

namespace Battlescript;

public static class Operator
{
    public static void Assign(CallStack callStack, Closure closure, string operation, VariableInstruction left, Instruction? right, Instruction? originalInstruction = null)
    {
        var rightVariable = right?.Interpret(callStack, closure);

        if (operation == "=")
        {
            closure.SetVariable(callStack, left, rightVariable!);
            return;
        }

        var leftVariable = left.Interpret(callStack, closure);
        var result = Operate(callStack, closure, operation, leftVariable, rightVariable, originalInstruction);
        
        // Unfortunately, these conversions cannot be done anywhere else.  Because we're modifying an existing variable,
        // we can't just change the value of the numeric like we do for other operations. We need to create a new
        // variable of the proper type.
        var finalResult = ApplyTypeConversionForAssignment(operation, result);
        closure.SetVariable(callStack, left, finalResult);
    }

    private static Variable ApplyTypeConversionForAssignment(string operation, Variable result)
    {
        if (operation == "/=" && BtlTypes.Is(BtlTypes.Types.Int, result))
        {
            return ConvertToBtlFloat(result);
        }

        if (operation == "//=" && BtlTypes.Is(BtlTypes.Types.Float, result))
        {
            return ConvertToBtlInt(result);
        }

        return result;
    }

    private static ObjectVariable ConvertToBtlFloat(Variable variable)
    {
        var objectResult = (ObjectVariable)variable;
        var numericValue = ((NumericVariable)objectResult.Values["__btl_value"]).Value;
        return BtlTypes.Create(BtlTypes.Types.Float, numericValue);
    }

    private static ObjectVariable ConvertToBtlInt(Variable variable)
    {
        var objectResult = (ObjectVariable)variable;
        var numericValue = ((NumericVariable)objectResult.Values["__btl_value"]).Value;
        return BtlTypes.Create(BtlTypes.Types.Int, numericValue);
    }

    private static NumericVariable CreateBoolNumeric(bool value) => new(value ? 1 : 0);
    
    public static Variable Operate(CallStack callStack, Closure closure, string operation, Variable? left, Variable? right, Instruction? originalInstruction = null)
    {
        if (BooleanOperators.Contains(operation))
        {
            return ConductBooleanOperation(callStack, closure, operation, left, right, originalInstruction);
        }

        if (left is ObjectVariable || right is ObjectVariable)
        {
            return ConductObjectOperation(callStack, closure, operation, left, right, originalInstruction);
        }

        if (left is StringVariable || right is StringVariable)
        {
            return ConductStringOperation(callStack, operation, left, right);
        }

        if (left is NumericVariable leftNumeric && right is NumericVariable rightNumeric)
        {
            return ConductBinaryNumericOperation(operation, leftNumeric, rightNumeric);
        }

        if (left is null && right is NumericVariable rightUnaryNumeric)
        {
            return ConductUnaryNumericOperation(operation, rightUnaryNumeric);
        }

        if (left is SequenceVariable leftSequence && right is SequenceVariable rightSequence)
        {
            return ConductBinarySequenceOperation(operation, leftSequence, rightSequence);
        }

        if (left is SequenceVariable leftSeq && right is NumericVariable rightNum)
        {
            return ConductNumericSequenceOperation(operation, leftSeq, rightNum);
        }

        if (left is NumericVariable leftNum && right is SequenceVariable rightSeq)
        {
            return ConductNumericSequenceOperation(operation, rightSeq, leftNum);
        }

        throw new InterpreterInvalidOperationException(operation, left, right);
    }
    
    private static Variable ConductBinarySequenceOperation(string operation, SequenceVariable leftSequence, SequenceVariable rightSequence)
    {
        return operation switch
        {
            "+" => new SequenceVariable(leftSequence.Values.Concat(rightSequence.Values).ToList()),
            "==" => CreateBoolNumeric(SequencesAreEqual(leftSequence, rightSequence)),
            "!=" => CreateBoolNumeric(!SequencesAreEqual(leftSequence, rightSequence)),
            _ => throw new InterpreterInvalidOperationException(operation, leftSequence, rightSequence)
        };
    }

    private static bool SequencesAreEqual(SequenceVariable sequence1, SequenceVariable sequence2)
    {
        if (sequence1.Values.Count != sequence2.Values.Count)
        {
            return false;
        }

        for (var i = 0; i < sequence1.Values.Count; i++)
        {
            var value1 = sequence1.Values[i];
            var value2 = sequence2.Values[i];

            if (value1 is null && value2 is null)
            {
                continue;
            }

            if (value1 is null || value2 is null)
            {
                return false;
            }

            if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }

    private static Variable ConductNumericSequenceOperation(string operation, SequenceVariable sequence, NumericVariable numeric)
    {
        return operation switch
        {
            "*" => MultiplySequence(sequence, numeric),
            "==" => new NumericVariable(0),
            _ => throw new InterpreterInvalidOperationException(operation, sequence, numeric)
        };
    }

    private static SequenceVariable MultiplySequence(SequenceVariable sequence, NumericVariable numeric)
    {
        var count = (int)numeric.Value;
        var values = new List<Variable?>(capacity: sequence.Values.Count * count);

        for (var i = 0; i < count; i++)
        {
            values.AddRange(sequence.Values);
        }

        return new SequenceVariable(values);
    }


    private static Variable ConductBooleanOperation(CallStack callStack, Closure closure, string operation,
        Variable? left, Variable? right, Instruction originalInstruction)
    {
        var leftTruthiness = Truthiness.IsTruthy(callStack, closure, left!, originalInstruction);
        var rightTruthiness = Truthiness.IsTruthy(callStack, closure, right!, originalInstruction);

        return operation switch
        {
            "or" => BtlTypes.Create(BtlTypes.Types.Bool, leftTruthiness || rightTruthiness),
            "and" => BtlTypes.Create(BtlTypes.Types.Bool, leftTruthiness && rightTruthiness),
            "not" => BtlTypes.Create(BtlTypes.Types.Bool, !rightTruthiness),
            "is" => BtlTypes.Create(BtlTypes.Types.Bool, ReferenceEquals(left, right)),
            "is not" => BtlTypes.Create(BtlTypes.Types.Bool, !ReferenceEquals(left, right)),
            "in" => BtlTypes.Create(BtlTypes.Types.Bool, ConductInOperation(operation, left, right)),
            "not in" => BtlTypes.Create(BtlTypes.Types.Bool, !ConductInOperation(operation, left, right)),
            _ => throw new InterpreterInvalidOperationException(operation, left, right)
        };
    }

    private static bool ConductInOperation(string operation, Variable? left, Variable? right)
    {
        if (BtlTypes.Is(BtlTypes.Types.String, left) && BtlTypes.Is(BtlTypes.Types.String, right))
        {
            return BtlTypes.GetStringValue(right).Contains(BtlTypes.GetStringValue(left));
        }

        if (BtlTypes.Is(BtlTypes.Types.List, right))
        {
            var listValue = BtlTypes.GetListValue(right);
            return listValue.Values.Any(x => x.Equals(left));
        }

        if (IsValidDictionaryInExpression(left, right))
        {
            return CheckDictionaryContainsKey(left, right);
        }

        throw new InterpreterInvalidOperationException(operation, left, right);
    }

    private static bool IsValidDictionaryInExpression(Variable? left, Variable? right)
    {
        var isLeftStringOrInt = BtlTypes.Is(BtlTypes.Types.String, left) || BtlTypes.Is(BtlTypes.Types.Int, left);
        return BtlTypes.Is(BtlTypes.Types.Dictionary, right) && isLeftStringOrInt;
    }

    private static bool CheckDictionaryContainsKey(Variable? left, Variable? right)
    {
        var dictValue = BtlTypes.GetDictValue(right);

        if (BtlTypes.Is(BtlTypes.Types.Int, left))
        {
            var intValue = BtlTypes.GetIntValue(left);
            return dictValue.IntValues.Any(x => x.Key.Equals(intValue));
        }

        var stringValue = BtlTypes.GetStringValue(left);
        return dictValue.StringValues.Any(x => x.Key.Equals(stringValue));
    }

    private static Variable ConductObjectOperation(CallStack callStack, Closure closure, string operation, Variable? left, Variable? right, Instruction? originalInstruction = null)
    {
        var leftOverride = GetLeftOperatorOverride(callStack, closure, operation, left, right);
        if (leftOverride is not null)
        {
            var args = new ArgumentSet(right is null ? [] : [right]);
            return leftOverride.RunFunction(callStack, closure, args, originalInstruction);
        }

        var rightOverride = GetRightOperatorOverride(callStack, closure, operation, left, right);
        if (rightOverride is not null)
        {
            var args = new ArgumentSet(left is null ? [] : [left]);
            return rightOverride.RunFunction(callStack, closure, args, originalInstruction);
        }

        throw new InterpreterInvalidOperationException(operation, left, right);
    }

    private static FunctionVariable? GetLeftOperatorOverride(
        CallStack callStack, Closure closure, string operation, Variable? left, Variable? right)
    {
        if (left is not ObjectVariable leftObject)
        {
            return null;
        }

        var isUnary = right is null;
        var overrideName = GetOverrideName(operation, isUnary, isReversed: false)
            ?? throw new InterpreterInvalidOperationException(operation, left, right);

        return leftObject.GetMember(callStack, closure, new MemberInstruction(overrideName)) as FunctionVariable;
    }

    private static FunctionVariable? GetRightOperatorOverride(
        CallStack callStack, Closure closure, string operation, Variable? left, Variable? right)
    {
        if (right is not ObjectVariable rightObject)
        {
            return null;
        }

        var isUnary = left is null;
        var isReversed = left is not null && ReversedOperationToOverrideMap.ContainsKey(operation);
        var overrideName = GetOverrideName(operation, isUnary, isReversed)
            ?? throw new InterpreterInvalidOperationException(operation, left, right);

        return rightObject.GetMember(callStack, closure, new MemberInstruction(overrideName)) as FunctionVariable;
    }

    private static string? GetOverrideName(string operation, bool isUnary, bool isReversed)
    {
        string? overrideName;

        if (isUnary)
        {
            UnaryOperationToOverrideMap.TryGetValue(operation, out overrideName);
        }
        else if (isReversed)
        {
            ReversedOperationToOverrideMap.TryGetValue(operation, out overrideName);
        }
        else
        {
            OperationToOverrideMap.TryGetValue(operation, out overrideName);
        }

        return overrideName;
    }
        
    private static Variable ConductStringOperation(CallStack callStack, string operation, Variable? left, Variable? right)
    {
        return operation switch
        {
            "+" => ConcatenateStrings(operation, left, right),
            "==" => CompareStringsForEquality(operation, left, right, expectEqual: true),
            "!=" => CompareStringsForEquality(operation, left, right, expectEqual: false),
            "*" => MultiplyString(operation, left, right),
            _ => throw new InterpreterInvalidOperationException(operation, left, right)
        };
    }

    private static Variable ConcatenateStrings(string operation, Variable? left, Variable? right)
    {
        var leftString = ConvertVariableToString(operation, left, right);
        var rightString = ConvertVariableToString(operation, right, left);
        return new StringVariable(leftString + rightString);
    }

    private static Variable CompareStringsForEquality(string operation, Variable? left, Variable? right, bool expectEqual)
    {
        if (left is StringVariable leftString && right is StringVariable rightString)
        {
            var isEqual = leftString.Value == rightString.Value;
            return CreateBoolNumeric(expectEqual ? isEqual : !isEqual);
        }

        throw new InterpreterInvalidOperationException(operation, left, right);
    }

    private static Variable MultiplyString(string operation, Variable? left, Variable? right)
    {
        return (left, right) switch
        {
            (StringVariable stringVar, NumericVariable numericVar) => RepeatString(stringVar, numericVar),
            (NumericVariable numericVar, StringVariable stringVar) => RepeatString(stringVar, numericVar),
            _ => throw new InterpreterInvalidOperationException(operation, left, right)
        };
    }

    private static Variable RepeatString(StringVariable stringVariable, NumericVariable numericVariable)
    {
        var result = string.Concat(Enumerable.Repeat(stringVariable.Value, (int)numericVariable.Value));
        return new StringVariable(result);
    }

    private static string ConvertVariableToString(string operation, Variable? variable, Variable? otherVariable)
    {
        return variable switch
        {
            StringVariable stringVar => stringVar.Value,
            NumericVariable numericVar => numericVar.Value.ToString(),
            _ => throw new InterpreterInvalidOperationException(operation, variable, otherVariable)
        };
    }
        
    private static Variable ConductBinaryNumericOperation(string operation, NumericVariable leftNumeric, NumericVariable rightNumeric)
    {
        return operation switch
        {
            "**" => new NumericVariable(Math.Pow(leftNumeric.Value, rightNumeric.Value)),
            "*" => new NumericVariable(leftNumeric.Value * rightNumeric.Value),
            "/" => new NumericVariable((double)leftNumeric.Value / (double)rightNumeric.Value),
            "//" => new NumericVariable(Math.Floor((double)leftNumeric.Value / (double)rightNumeric.Value)),
            "%" => new NumericVariable(leftNumeric.Value % rightNumeric.Value),
            "+" => new NumericVariable(leftNumeric.Value + rightNumeric.Value),
            "-" => new NumericVariable(leftNumeric.Value - rightNumeric.Value),
            "==" => CreateBoolNumeric(Math.Abs(leftNumeric.Value - rightNumeric.Value) < Consts.FloatingPointTolerance),
            "!=" => CreateBoolNumeric(Math.Abs(leftNumeric.Value - rightNumeric.Value) > Consts.FloatingPointTolerance),
            ">" => CreateBoolNumeric(leftNumeric.Value > rightNumeric.Value),
            ">=" => CreateBoolNumeric(leftNumeric.Value >= rightNumeric.Value),
            "<" => CreateBoolNumeric(leftNumeric.Value < rightNumeric.Value),
            "<=" => CreateBoolNumeric(leftNumeric.Value <= rightNumeric.Value),
            "+=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => l + r),
            "-=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => l - r),
            "*=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => l * r),
            "/=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => l / (double)r),
            "//=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => Math.Floor((double)l / (double)r)),
            "%=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => l % r),
            "**=" => ApplyCompoundAssignment(leftNumeric, rightNumeric, (l, r) => Math.Pow(l, r)),
            _ => throw new InterpreterInvalidOperationException(operation, leftNumeric, rightNumeric)
        };
    }

    private static NoneVariable ApplyCompoundAssignment(NumericVariable leftNumeric, NumericVariable rightNumeric, Func<double, double, double> operation)
    {
        leftNumeric.Value = operation(leftNumeric.Value, rightNumeric.Value);
        return new NoneVariable();
    }
    
    private static Variable ConductUnaryNumericOperation(string operation, NumericVariable numeric)
    {
        return operation switch
        {
            "-" => new NumericVariable(-numeric.Value),
            "+" => new NumericVariable(numeric.Value),
            _ => throw new InterpreterInvalidOperationException(operation, null, numeric)
        };
    }
    
    #region Constants
    
    private static readonly Dictionary<string, string> OperationToOverrideMap = new()
    {
        { "+", "__add__" },
        { "-", "__sub__" },
        { "*", "__mul__" },
        { "/", "__truediv__" },
        { "//", "__floordiv__" },
        { "%", "__mod__" },
        { "**", "__pow__" },
        { "==", "__eq__" },
        { "!=", "__ne__" },
        { "<", "__lt__" },
        { "<=", "__le__" },
        { ">", "__gt__" },
        { ">=", "__ge__" },
        { "+=", "__iadd__" },
        { "-=", "__isub__" },
        { "*=", "__imul__" },
        { "/=", "__itruediv__" },
        { "//=", "__ifloordiv__" },
        { "%=", "__imod__" },
        { "**=", "__ipow__" },
    };

    private static readonly Dictionary<string, string> ReversedOperationToOverrideMap = new()
    {
        { "-", "__rsub__" },
        { "/", "__rtruediv__" },
        { "//", "__rfloordiv__" },
        { "%", "__rmod__" },
        { "**", "__rpow__" },
        { "<", "__rlt__" },
        { "<=", "__rle__" },
        { ">", "__rgt__" },
        { ">=", "__rge__" },
    };

    private static readonly Dictionary<string, string> UnaryOperationToOverrideMap = new()
    {
        { "+", "__pos__" },
        { "-", "__neg__" },
    };
    
    private static readonly FrozenSet<string> BooleanOperators =
        FrozenSet.ToFrozenSet(["and", "or", "not", "is", "is not", "in", "not in"]);
    
    #endregion
}