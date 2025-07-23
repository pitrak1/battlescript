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
        // This needs to be rewritten to support ranged assignments, look at list methods in python to see test cases
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
                } else if (BsTypes.Is(memory, BsTypes.Types.List, valueVariable))
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
                return GetRangeIndex(memory, indexSequence.Values);
            }
        } 
        else 
        {
            var indexInt = BsTypes.GetIntValue(memory, indexSequence.Values[0]);
            if (index.Next is null)
            {
                Values[indexInt] = valueVariable;
            }
            return valueVariable;
        }
    }
    
    public void SetRangeIndex(Memory memory, SequenceVariable valueVariable, List<Variable> argVariable)
    {
        var (start, stop, step) = GetRangeIndexValues(memory, argVariable);
        
        int index = start;
        int valueIndex = 0;
        if (step < 0)
        {
            while (index > stop)
            {
                Values[index] = valueVariable.Values[valueIndex];
                index += step;
                valueIndex += 1;
            }
        }
        else
        {
            while (index < stop)
            {
                Values[index] = valueVariable.Values[valueIndex];
                index += step;
                valueIndex += 1;
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
            return GetRangeIndex(memory, indexSequence.Values);
        }
        else
        {
            var indexInt = BsTypes.GetIntValue(memory, indexSequence.Values[0]);
            return Values[indexInt];
        }
    }
    
    public SequenceVariable GetRangeIndex(Memory memory, List<Variable> argVariable)
    {
        var (start, stop, step) = GetRangeIndexValues(memory, argVariable);
        
        int index = start;
        SequenceVariable result = new SequenceVariable();
        if (step < 0)
        {
            while (index > stop)
            {
                result.Values.Add(Values[index]);
                index += step;
            }
        }
        else
        {
            while (index < stop)
            {
                result.Values.Add(Values[index]);
                index += step;
            }
        }

        return result;
    }

    public (int start, int stop, int step) GetRangeIndexValues(Memory memory, List<Variable> argVariable)
    {
        int start = 0;
        int stop = Values.Count;
        int step = 1;

        if (argVariable.Count >= 3)
        {
            if (argVariable[2] is not null && argVariable[2] is not ConstantVariable { Value: Consts.Constants.None} )
            {
                step = BsTypes.GetIntValue(memory, argVariable[2]);
                
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
                stop = BsTypes.GetIntValue(memory, argVariable[1]) % Values.Count;
            }
        }
        
        if (argVariable.Count >= 1)
        {
            if (argVariable[0] is not null && argVariable[0] is not ConstantVariable { Value: Consts.Constants.None})
            {
                start = BsTypes.GetIntValue(memory, argVariable[0]) % Values.Count;
            }
        }
        
        return (start, stop, step);
    }

    public override Variable Operate(Memory memory, string operation, Variable? other, bool isInverted = false)
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