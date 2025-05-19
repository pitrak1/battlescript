namespace Battlescript;

public class NumberVariable(double value) : Variable, IEquatable<NumberVariable>
{
    public double Value { get; set; } = value;
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a number variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a number variable");
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as NumberVariable);
    public bool Equals(NumberVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}