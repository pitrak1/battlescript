using System.Diagnostics;

namespace Battlescript;

public class SequenceVariable : Variable, IEquatable<SequenceVariable>
{
    public List<Variable?> Values { get; set; }

    public SequenceVariable(List<Variable?>? values = null)
    {
        Values = values ?? [];
    }
    
    public override Variable? SetItemDirectly(Memory memory, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);
        var indexList = indexVariable as ObjectVariable;
        var indexSequence = indexList.Values["__value"] as SequenceVariable;

        if (indexSequence.Values.Count > 1)
        {
            if (index.Next is null)
            {
                if (valueVariable is SequenceVariable sequenceVariable)
                {
                    SetRangeIndex(memory, sequenceVariable, indexSequence.Values);
                } else if (memory.Is(Memory.BsTypes.List, valueVariable))
                {
                    SetRangeIndex(memory, (valueVariable as ObjectVariable).Values["__value"] as SequenceVariable, indexSequence.Values);
                }
                else
                {
                    throw new Exception("Cannot assign sequence to non-sequence");
                }
                return valueVariable;
            }
            else
            {
                return GetSlice(memory, indexSequence.Values);
            }
        } 
        else 
        {
            var indexInt = memory.GetIntValue(indexSequence.Values[0]);
            if (index.Next is null)
            {
                Values[indexInt] = valueVariable;
            }
            return valueVariable;
        }
    }
    
    public void SetRangeIndex(Memory memory, SequenceVariable valueVariable, List<Variable> argVariable)
    {
        var (start, stop, step) = GetSliceArgs(memory, argVariable);
        var indices = GetSliceIndices(memory, argVariable);

        if (step == 1)
        {
            var valuesBeforeSlice = Values.GetRange(0, start);
            var valuesAfterSlice = Values.GetRange(stop, Values.Count - stop);
            Values = valuesBeforeSlice.Concat(valueVariable.Values).Concat(valuesAfterSlice).ToList();
        }
        else
        {
            if (indices.Count != valueVariable.Values.Count)
            {
                throw new InternalRaiseException(Memory.BsTypes.ValueError, $"attempt to assign sequence of size {valueVariable.Values.Count} to extended slice of size {indices.Count}");
            }

            for (var i = 0; i < indices.Count; i++)
            {
                Values[indices[i]] = valueVariable.Values[i];
            }
        }
    }
    
    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(memory);
        // For single index, this is an int
        var indexList = indexVariable as ObjectVariable;
        var indexSequence = indexList.Values["__value"] as SequenceVariable;
        
        // var indexInt = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "int", indexVariable);
        if (indexSequence.Values.Count > 1)
        {
            return GetSlice(memory, indexSequence.Values);
        }
        else
        {
            var indexInt = memory.GetIntValue(indexSequence.Values[0]);
            return Values[indexInt];
        }
    }
    
    public SequenceVariable GetSlice(Memory memory, List<Variable> argVariable)
    {
        var indices = GetSliceIndices(memory, argVariable);
        SequenceVariable result = new SequenceVariable();
        foreach (var index in indices)
        {
            result.Values.Add(Values[index]);
        }

        return result;
    }

    public List<int> GetSliceIndices(Memory memory, List<Variable> argVariable)
    {
        var (start, stop, step) = GetSliceArgs(memory, argVariable);

        var index = start;
        List<int> indices = [];
        if (step == 0)
        {
            throw new InternalRaiseException(Memory.BsTypes.ValueError, "slice step cannot be zero");
        }
        else if (step < 0)
        {
            while (index > stop)
            {
                indices.Add(index);
                index += step;
            }
        }
        else
        {
            while (index < stop)
            {
                indices.Add(index);
                index += step;
            }
        }

        return indices;
    }

    public (int start, int stop, int step) GetSliceArgs(Memory memory, List<Variable?> argVariable)
    {
        int start = 0;
        int stop = Values.Count;
        int step = 1;

        if (argVariable.Count >= 3)
        {
            if (argVariable[2] is not null && argVariable[2] is not ConstantVariable { Value: Consts.Constants.None} )
            {
                step = memory.GetIntValue(argVariable[2]!);
                
                if (step < 0)
                {
                    start = Values.Count - 1;
                    stop = -1;
                }
            }
        }
        
        if (argVariable.Count >= 2)
        {
            if (argVariable[1] is not null && argVariable[1] is not ConstantVariable { Value: Consts.Constants.None})
            {
                var rawInt = memory.GetIntValue(argVariable[1]!);
                stop = Math.Clamp(rawInt, -Values.Count, Values.Count);
                if (stop < 0)
                {
                    stop += Values.Count;
                }
            }
        }
        
        if (argVariable.Count >= 1)
        {
            if (argVariable[0] is not null && argVariable[0] is not ConstantVariable { Value: Consts.Constants.None})
            {
                var rawInt = memory.GetIntValue(argVariable[0]!);
                start = Math.Clamp(rawInt, -Values.Count, Values.Count);
                if (start < 0)
                {
                    start += Values.Count;
                }
            }
        }
        
        return (start, stop, step);
    }

    public override Variable Operate(Memory memory, string operation, Variable? other, bool isTransposed = false)
    {
        if (other is NumericVariable otherNumeric)
        {
            switch (operation)
            {
                case "*":
                    return MultiplySequence(otherNumeric);
                case "==":
                    return new NumericVariable(0);
                default:
                    throw new InterpreterInvalidOperationException(operation, this, other);
            }
        } 
        else if (other is SequenceVariable otherSequence)
        {
            switch (operation)
            {
                case "+":
                    return new SequenceVariable(Values.Concat(otherSequence.Values).ToList());
                case "==":
                    return CompareSequences(otherSequence) ? new NumericVariable(1) : new NumericVariable(0);
                default:
                    throw new InterpreterInvalidOperationException(operation, this, other);
            }
        }
        else if (other is null)
        {
            throw new InterpreterInvalidOperationException(operation, this, other);
        }
        else
        {
            return other.Operate(memory, operation, this);
        }

        bool SequenceContains(Variable? value)
        {
            for (var i = 0; i < Values.Count; i++)
            {
                if (Values[i].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }
        
        bool CompareSequences(SequenceVariable otherSequence)
        {
            if (Values.Count != otherSequence.Values.Count)
            {
                return false;
            }
            
            for (var i = 0; i < Values.Count; i++)
            {
                if (!Values[i].Equals(otherSequence.Values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        Variable MultiplySequence(NumericVariable otherNumeric)
        {
            var values = new List<Variable>();
            for (var i = 0; i < otherNumeric.Value; i++)
            {
                foreach (var value in Values)
                {
                    values.Add(value);
                }
            }
            return new SequenceVariable(values);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as SequenceVariable);
    public bool Equals(SequenceVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Values.SequenceEqual(variable.Values);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values);
}