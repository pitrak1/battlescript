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
    
    public override Variable? SetItemDirectly(CallStack callStack, Closure closure, Variable valueVariable, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexValue = GetIndexValue(callStack, closure, index);
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

    private (int? IntValue, string? StringValue) GetIndexValue(CallStack callStack, Closure closure, ArrayInstruction index)
    {
        var indexVariable = index.Values[0].Interpret(callStack, closure);
        var indexList = indexVariable as ObjectVariable;
        var indexSequence = indexList.Values["__btl_value"] as SequenceVariable;
        
        if (BtlTypes.Is(BtlTypes.Types.Int, indexSequence.Values[0]))
        {
            return (BtlTypes.GetIntValue(indexSequence.Values[0]), null);
        } else if (BtlTypes.Is(BtlTypes.Types.String, indexSequence.Values[0]))
        {
            return (null, BtlTypes.GetStringValue(indexSequence.Values[0]));
        }
        else
        {
            throw new Exception("Invlaid dictionary index, must be int or string");
        }
    }

    public override Variable? GetItemDirectly(CallStack callStack, Closure closure, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexValue = GetIndexValue(callStack, closure, index);
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
    
    #region Equality

    public override bool Equals(object? obj) => obj is MappingVariable variable && Equals(variable);

    public bool Equals(MappingVariable? other) =>
        other is not null &&
        IntValues.OrderBy(kvp => kvp.Key).SequenceEqual(other.IntValues.OrderBy(kvp => kvp.Key)) &&
        StringValues.OrderBy(kvp => kvp.Key).SequenceEqual(other.StringValues.OrderBy(kvp => kvp.Key));

    public override bool Equals(Variable? other) => other is MappingVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(IntValues, StringValues);

    public static bool operator ==(MappingVariable? left, MappingVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(MappingVariable? left, MappingVariable? right) => !(left == right);

    #endregion
}