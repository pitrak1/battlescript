using System.Diagnostics;

namespace Battlescript;

public class MappingVariable : Variable, IEquatable<MappingVariable>
{ 
    public Dictionary<int, Variable> IntValues { get; set; }
    public Dictionary<string, Variable> StringValues { get; set; }

    public MappingVariable(Dictionary<int, Variable>? intValues = null, Dictionary<string, Variable>? stringValues = null)
    {
        IntValues = intValues ?? new Dictionary<int, Variable>();
        StringValues = stringValues ?? new Dictionary<string, Variable>();
    }
    
    public override Variable? SetItemDirectly(Memory memory, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexValue = GetIndexValue(memory, index);
        if (indexValue.IntValue is not null)
        {
            var intValue = indexValue.IntValue.Value;
            if (index.Next is null)
            {
                IntValues[intValue] = valueVariable;
                return valueVariable;
            }
            else
            {
                return IntValues[intValue];
            }
        }
        else
        {
            var stringValue = indexValue.StringValue;
            if (index.Next is null)
            {
                StringValues[stringValue!] = valueVariable;
                return valueVariable;
            }
            else
            {
                return StringValues[stringValue!];
            }
        }
    }

    private (int? IntValue, string? StringValue) GetIndexValue(Memory memory, ArrayInstruction index)
    {
        var indexVariable = index.Values[0].Interpret(memory);
        var indexList = indexVariable as ObjectVariable;
        var indexSequence = indexList.Values["__value"] as SequenceVariable;
        
        if (memory.Is(Memory.BsTypes.Int, indexSequence.Values[0]))
        {
            return (memory.GetIntValue(indexSequence.Values[0]), null);
        } else if (memory.Is(Memory.BsTypes.String, indexSequence.Values[0]))
        {
            return (null, memory.GetStringValue(indexSequence.Values[0]));
        }
        else
        {
            throw new Exception("Invlaid dictionary index, must be int or string");
        }
    }

    public override Variable? GetItemDirectly(Memory memory, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexValue = GetIndexValue(memory, index);
        if (indexValue.IntValue is not null)
        {
            var intValue = indexValue.IntValue.Value;
            return IntValues[intValue];
        }
        else
        {
            var stringValue = indexValue.StringValue;
            return StringValues[stringValue!];
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as MappingVariable);
    public bool Equals(MappingVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        var intEqual = IntValues.OrderBy(kvp => kvp.Key).SequenceEqual(variable.IntValues.OrderBy(kvp => kvp.Key));
        var stringEqual = StringValues.OrderBy(kvp => kvp.Key).SequenceEqual(variable.StringValues.OrderBy(kvp => kvp.Key));
        return intEqual && stringEqual;
    }
    
    public override int GetHashCode() => HashCode.Combine(IntValues, StringValues);
}