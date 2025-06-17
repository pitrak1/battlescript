namespace Battlescript;

public class StringVariable(string? value = null) : ValueVariable, IEquatable<StringVariable>
{
    public string Value { get; set; } = value ?? "";
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a string variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a string variable");
    }
    
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