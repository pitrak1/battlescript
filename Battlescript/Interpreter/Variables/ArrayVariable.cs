namespace Battlescript;

public class ArrayVariable(string? separator = null, List<Variable?>? values = null) : ReferenceVariable, IEquatable<ArrayVariable>
{
    public string Separator { get; set; } = separator ?? ",";
    public List<Variable?> Values { get; set; } = values ?? [];

    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ArrayVariable);
    public bool Equals(ArrayVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        
        return Separator.Equals(variable.Separator) && Values.SequenceEqual(variable.Values);
    }
    
    public override int GetHashCode() => HashCode.Combine(Values);
}