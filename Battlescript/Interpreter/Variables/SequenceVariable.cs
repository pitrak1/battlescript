using System.Diagnostics;

namespace Battlescript;

public class SequenceVariable : Variable, IEquatable<SequenceVariable>
{
    public List<Variable?> Values { get; set; }

    public SequenceVariable(List<Variable?>? values = null)
    {
        Values = values ?? [];
    }
    
    public override Variable? SetItemDirectly(CallStack callStack, Closure closure, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        // Unfortunately, the way this is structured is that the index here will be a list of lists, first separated by 
        // commas then by colons.  Because we don't expect any commas, we need to only take the first index and convert
        // to an array of Variables.
        var indexValuesList = index.Values[0].Interpret(callStack, closure) as ObjectVariable;
        var indexValuesSequence = BsTypes.GetListValue(indexValuesList);

        if (indexValuesSequence.Values.Count > 1)
        {
            if (index.Next is null)
            {
                if (valueVariable is SequenceVariable sequenceVariable)
                {
                    SetRangeIndex(callStack, sequenceVariable, indexValuesSequence.Values);
                } else if (BsTypes.Is(BsTypes.Types.List, valueVariable))
                {
                    SetRangeIndex(callStack, (valueVariable as ObjectVariable).Values["__value"] as SequenceVariable, indexValuesSequence.Values);
                }
                else
                {
                    throw new Exception("Cannot assign sequence to non-sequence");
                }
                return valueVariable;
            }
            else
            {
                return GetSlice(callStack, indexValuesSequence.Values);
            }
        } 
        else 
        {
            var indexInt = BsTypes.GetIntValue(indexValuesSequence.Values[0]);
            if (index.Next is null)
            {
                Values[indexInt] = valueVariable;
            }
            return valueVariable;
        }
    }
    
    public void SetRangeIndex(CallStack callStack, SequenceVariable valueVariable, List<Variable> argVariable)
    {
        var (start, stop, step) = GetSliceArgs(callStack, argVariable);
        var indices = GetSliceIndices(callStack, argVariable);

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
                throw new InternalRaiseException(BsTypes.Types.ValueError, $"attempt to assign sequence of size {valueVariable.Values.Count} to extended slice of size {indices.Count}");
            }

            for (var i = 0; i < indices.Count; i++)
            {
                Values[indices[i]] = valueVariable.Values[i];
            }
        }
    }
    
    public override Variable? GetItemDirectly(CallStack callStack, Closure closure, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(callStack, closure);
        // For single index, this is an int
        var indexList = indexVariable as ObjectVariable;
        var indexSequence = indexList.Values["__value"] as SequenceVariable;
        
        // var indexInt = BuiltInTypeHelper.IsVariableBuiltInClass(callStack, "int", indexVariable);
        if (indexSequence.Values.Count > 1)
        {
            return GetSlice(callStack, indexSequence.Values);
        }
        else
        {
            var indexInt = BsTypes.GetIntValue(indexSequence.Values[0]);
            return Values[indexInt];
        }
    }
    
    public SequenceVariable GetSlice(CallStack callStack, List<Variable> argVariable)
    {
        var indices = GetSliceIndices(callStack, argVariable);
        SequenceVariable result = new SequenceVariable();
        foreach (var index in indices)
        {
            result.Values.Add(Values[index]);
        }

        return result;
    }

    public List<int> GetSliceIndices(CallStack callStack, List<Variable> argVariable)
    {
        var (start, stop, step) = GetSliceArgs(callStack, argVariable);

        var index = start;
        List<int> indices = [];
        if (step == 0)
        {
            throw new InternalRaiseException(BsTypes.Types.ValueError, "slice step cannot be zero");
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

    public (int start, int stop, int step) GetSliceArgs(CallStack callStack, List<Variable?> argVariable)
    {
        int start = 0;
        int stop = Values.Count;
        int step = 1;

        if (argVariable.Count >= 3)
        {
            if (argVariable[2] is not null && argVariable[2] is not ConstantVariable { Value: Consts.Constants.None} )
            {
                step = BsTypes.GetIntValue(argVariable[2]!);
                
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
                var rawInt = BsTypes.GetIntValue(argVariable[1]!);
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
                var rawInt = BsTypes.GetIntValue(argVariable[0]!);
                start = Math.Clamp(rawInt, -Values.Count, Values.Count);
                if (start < 0)
                {
                    start += Values.Count;
                }
            }
        }
        
        return (start, stop, step);
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