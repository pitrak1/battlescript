using System.Diagnostics;

namespace Battlescript;

public class DictionaryVariable : Variable, IEquatable<DictionaryVariable>
{ 
    public Dictionary<int, Variable> IntValues { get; set; }
    public Dictionary<string, Variable> StringValues { get; set; }

    public DictionaryVariable(Dictionary<int, Variable>? intValues = null, Dictionary<string, Variable>? stringValues = null)
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
        var indexVariable = index.Values.Select(x => x.Interpret(memory)).ToList();
        
        var intObject = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "int", indexVariable[0]);
        if (intObject is not null)
        {
            return (BuiltInTypeHelper.GetIntValueFromVariable(memory, indexVariable[0]), null);
        } else if (indexVariable[0] is StringVariable stringVariable)
        {
            return (null, stringVariable.Value);
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
    public override bool Equals(object obj) => Equals(obj as DictionaryVariable);
    public bool Equals(DictionaryVariable? variable)
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