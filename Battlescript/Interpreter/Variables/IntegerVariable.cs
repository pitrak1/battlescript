namespace Battlescript;

public class IntegerVariable(int value) : ValueVariable
{
    public int Value { get; set; } = value;
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a number variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a number variable");
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