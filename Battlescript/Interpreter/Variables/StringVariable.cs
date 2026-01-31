namespace Battlescript;

public class StringVariable : Variable, IEquatable<StringVariable>
{
    public string Value { get; set; }

    public StringVariable(string? value = null)
    {
        Value = value ?? "";
    }

    public SequenceVariable ToSequence() =>
        new SequenceVariable(Value.Select(c => new StringVariable(c.ToString())).Cast<Variable>().ToList());
    
    public SequenceVariable ToBtlSequence() =>
        new SequenceVariable(Value.Select(c => BtlTypes.Create(BtlTypes.Types.String, c.ToString())).ToList());
    
    public override Variable Copy() => new StringVariable(Value);

    public override Variable? GetItemDirectly(CallStack callStack, Closure closure, ArrayInstruction index, ObjectVariable? objectContext = null)
    {
        var indexVariable = index.Values[0].Interpret(callStack, closure);
        var indexList = indexVariable as ObjectVariable;
        var indexSequence = indexList!.Values["__btl_value"] as SequenceVariable;

        if (indexSequence!.Values.Count > 1)
        {
            // Slicing: s[1:3] returns a substring
            return GetSlice(indexSequence.Values);
        }
        else
        {
            // Single index: s[0] returns a single character string
            var indexInt = BtlTypes.GetIntValue(indexSequence.Values[0]);
            if (indexInt < 0)
                indexInt = Value.Length + indexInt;
            if (indexInt < 0 || indexInt >= Value.Length)
                throw new InternalRaiseException(BtlTypes.Types.IndexError, "string index out of range");
            return new StringVariable(Value[indexInt].ToString());
        }
    }

    private StringVariable GetSlice(List<Variable?> argVariable)
    {
        var indices = SliceHelper.GetSliceIndices(argVariable, Value.Length);
        var result = new System.Text.StringBuilder();
        foreach (var i in indices)
        {
            result.Append(Value[i]);
        }
        return new StringVariable(result.ToString());
    }

    #region Equality

    public override bool Equals(object? obj) => obj is StringVariable variable && Equals(variable);

    public bool Equals(StringVariable? other) =>
        other is not null && Value == other.Value;

    public override bool Equals(Variable? other) => other is StringVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Value);

    public static bool operator ==(StringVariable? left, StringVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(StringVariable? left, StringVariable? right) => !(left == right);

    #endregion
}