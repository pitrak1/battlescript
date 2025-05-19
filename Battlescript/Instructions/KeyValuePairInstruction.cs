namespace Battlescript;

public class KeyValuePairInstruction : Instruction, IEquatable<KeyValuePairInstruction>
{
    public Instruction Left { get; set; } 
    public Instruction Right { get; set; }

    public KeyValuePairInstruction(List<Token> tokens)
    {
        var colonIndex = ParserUtilities.GetTokenIndex(tokens, [":"]);
        var colonToken = tokens[colonIndex];
        var result = RunLeftAndRightAroundIndex(tokens, colonIndex);
        
        Left = result.Left;
        Right = result.Right;
        Line = colonToken.Line;
        Column = colonToken.Column;
    }

    public KeyValuePairInstruction(Instruction left, Instruction right)
    {
        Left = left;
        Right = right;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        return new KeyValuePairVariable(Left?.Interpret(memory, context), Right?.Interpret(memory, context));
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as KeyValuePairInstruction);
    public bool Equals(KeyValuePairInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (Left != instruction.Left || Right != instruction.Right) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Left, Right, Instructions);
    public static bool operator ==(KeyValuePairInstruction left, KeyValuePairInstruction right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(KeyValuePairInstruction left, KeyValuePairInstruction right) => !(left == right);
}