namespace Battlescript;

public class StringVariable(string? value = null) : ValueVariable, IEquatable<StringVariable>
{
    public string Value { get; set; } = value ?? "";
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as StringVariable);
    public bool Equals(StringVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}