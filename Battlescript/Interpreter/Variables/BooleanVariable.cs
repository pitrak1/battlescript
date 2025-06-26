namespace Battlescript;

public class BooleanVariable(bool? value = null) : ValueVariable, IEquatable<BooleanVariable>
{
    public bool Value { get; set; } = value ?? false;
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as BooleanVariable);
    public bool Equals(BooleanVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}