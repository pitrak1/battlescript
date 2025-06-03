namespace Battlescript;

public class StringInstruction : Instruction, IEquatable<StringInstruction>
{
    public string Value { get; set; }

    public StringInstruction(List<Token> tokens)
    {
        Value = tokens[0].Value;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public StringInstruction(string value)
    {
        Value = value;
    }
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new StringVariable(Value);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as StringInstruction);
    public bool Equals(StringInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}