namespace Battlescript;

public class FloatInstruction : Instruction, IEquatable<FloatInstruction>
{
    public double Value { get; set; }

    public FloatInstruction(List<Token> tokens)
    {
        if (tokens.Count > 1)
        {
            throw new ParserUnexpectedTokenException(tokens[1]);
        }
        
        Value = Convert.ToDouble(tokens[0].Value);
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public FloatInstruction(double value)
    {
        Value = value;
    }
    
    public override Variable Interpret(        
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new FloatVariable(Value);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as FloatInstruction);
    public bool Equals(FloatInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}