namespace Battlescript;

public class ConstantVariable : Variable, IEquatable<ConstantVariable>
{
    public Consts.Constants Value { get; set; }

    public ConstantVariable(Consts.Constants value)
    {
        Value = value;
        Type = Consts.VariableTypes.Value;
    }

    public ConstantVariable(bool? value = null)
    {
        if (value is null)
        {
            Value = Consts.Constants.None;
        } else if (value == true)
        {
            Value = Consts.Constants.True;
        }
        else
        {
            Value = Consts.Constants.False;
        }
        Type = Consts.VariableTypes.Value;
    }
    
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