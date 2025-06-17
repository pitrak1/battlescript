namespace Battlescript;

public class BooleanVariable(bool? value = null) : ValueVariable, IEquatable<BooleanVariable>
{
    public bool Value { get; set; } = value ?? false;
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a boolean variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a boolean variable");
    }
    
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