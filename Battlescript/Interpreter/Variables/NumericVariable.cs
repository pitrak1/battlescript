namespace Battlescript;

public class NumericVariable : Variable
{
    private dynamic _value;
    public dynamic Value
    {
        get => _value;
        set
        {
            if (value is not double && value is not int)
            {
                throw new Exception("Wrong type for numeric instruction");
            }
            else
            {
                _value = value;
            }
        }
    }

    public NumericVariable(dynamic value)
    {
        _value = value;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as NumericVariable);
    public bool Equals(NumericVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}