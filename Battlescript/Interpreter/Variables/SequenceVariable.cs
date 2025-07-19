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
        var indexVariable = index.Values.Select(x => x.Interpret(memory)).ToList();

        var indexInt = BuiltInTypeHelper.GetIntValueFromVariable(memory, indexVariable[0]);
        if (index.Next is null)
        {
            Values[indexInt] = valueVariable;
            return valueVariable;
        }
        else
        {
            return Values[indexInt];
        }
    }
    
    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values.Select(x => x.Interpret(memory)).ToList();

        if (indexVariable.Count > 1)
        {
            return GetRangeIndex(memory, indexVariable);
        }
        else
        {
            var indexInt = BuiltInTypeHelper.GetIntValueFromVariable(memory, indexVariable[0]);
            return Values[indexInt];
        }
    }
    
    public SequenceVariable GetRangeIndex(Memory memory, List<Variable> argVariable)
    {
        int start = 0;
        int stop = Values.Count;
        int step = 1;
        
        List<int?> values = [];
        foreach (var value in argVariable)
        {
            if (value is null)
            {
                values.Add(null);
            }
            else
            {
                var indexInt = BuiltInTypeHelper.GetIntValueFromVariable(memory, value);
                values.Add(indexInt);
            }
        }
        
        if (values.Count == 3)
        {
            if (values[2] is not null)
            {
                step = values[2] ?? 1;
            }

            if (values[0] is not null)
            {
                start = values[0] % Values.Count ?? 0;
            } else if (values[0] is null && step < 0)
            {
                start = Values.Count - 1;
            }

            if (values[1] is not null)
            {
                stop = values[1] % Values.Count ?? Values.Count;
            }
            else if (values[1] is null && step < 0)
            {
                stop = -1;
            }
        } else if (values.Count == 2)
        {
            if (values[0] is not null)
            {
                start = values[0] % Values.Count ?? 0;
            }

            if (values[1] is not null)
            {
                stop = values[1] % Values.Count ?? Values.Count;
            }
        }
        else
        {
            throw new Exception("Invalid range index, fix this later");
        }

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

    public Variable? Operate(Memory memory, string operation, Variable? other)
    {
        switch (operation)
        {
            case "+":
                if (other is SequenceVariable sequenceVariable)
                {
                    return new SequenceVariable(Values.Concat(sequenceVariable.Values).ToList());
                }
                else
                {
                    throw new Exception("Cannot add sequence and non-sequence");
                }
            case "*":
                var intVariable = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "int", other);
                if (intVariable is not null)
                {
                    var intValue = BuiltInTypeHelper.GetIntValueFromVariable(memory, other);
                    var values = new List<Variable>();
                    for (var i = 0; i < intValue; i++)
                    {
                        foreach (var value in Values)
                        {
                            values.Add(value);
                        }
                    }
                    return new SequenceVariable(values);
                }
                else
                {
                    throw new Exception("Cannot multiply sequence and non-int");
                }
            default:
                throw new Exception("Invalid operation");
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