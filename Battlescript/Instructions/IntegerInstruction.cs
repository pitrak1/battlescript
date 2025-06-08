namespace Battlescript;

public class IntegerInstruction : Instruction, IEquatable<IntegerInstruction>
{
    public int Value { get; set; }

    public IntegerInstruction(List<Token> tokens)
    {
        if (tokens.Count > 1)
        {
            throw new ParserUnexpectedTokenException(tokens[1]);
        }
        
        Value = Convert.ToInt32(tokens[0].Value);
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public IntegerInstruction(int value)
    {
        Value = value;
    }
    
    public override Variable Interpret(        
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new IntegerVariable(Value);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as IntegerInstruction);
    public bool Equals(IntegerInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Value != instruction.Value) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}