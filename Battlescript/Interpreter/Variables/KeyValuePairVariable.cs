namespace Battlescript;

public class KeyValuePairVariable(Variable? left = null, Variable? right = null) : Variable, IEquatable<KeyValuePairVariable>
{
    public Variable? Left { get; set; } = left;
    public Variable? Right { get; set; } = right;
    
    public override void SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index)
    {
        throw new Exception("Cannot index a kvp variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a kvp variable");
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as KeyValuePairVariable);
    public bool Equals(KeyValuePairVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        
        return Left.Equals(variable.Left) && Right.Equals(variable.Right);
    }
    
    public override int GetHashCode() => HashCode.Combine(Left, Right);
}