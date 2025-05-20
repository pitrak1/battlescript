namespace Battlescript;

public class BooleanInstruction : Instruction, IEquatable<BooleanInstruction>
{
    public bool Value { get; set; }

    public BooleanInstruction(List<Token> tokens)
    {
        Value = tokens[0].Value == "True";
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public BooleanInstruction(bool value)
    {
        Value = value;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        return new BooleanVariable(Value);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as BooleanInstruction);
    public bool Equals(BooleanInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}