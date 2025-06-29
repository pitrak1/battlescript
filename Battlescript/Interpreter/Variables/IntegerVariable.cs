namespace Battlescript;

public class IntegerVariable : Variable
{
    public int Value { get; set; }

    public IntegerVariable(int value)
    {
        Value = value;
        Type = Consts.VariableTypes.Value;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as IntegerVariable);
    public bool Equals(IntegerVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}