namespace Battlescript;

public class ConstantVariable : Variable, IEquatable<ConstantVariable>
{
    public Consts.Constants Value { get; set; } = Consts.Constants.None;
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ConstantVariable);
    public bool Equals(ConstantVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}