namespace Battlescript;

public class FloatVariable : Variable
{
    public double Value { get; set; }

    public FloatVariable(double value)
    {
        Value = value;
        Type = Consts.VariableTypes.Value;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as FloatVariable);
    public bool Equals(FloatVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}