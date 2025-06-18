using System.Diagnostics;

namespace Battlescript;

public class ReturnInstruction : Instruction, IEquatable<ReturnInstruction>
{
    public Instruction? Value { get; set; }

    public ReturnInstruction(List<Token> tokens)
    {
        if (tokens.Count > 1)
        {
            Value = Parse(tokens.GetRange(1, tokens.Count - 1));
        }
        
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ReturnInstruction(Instruction? value)
    {
        Value = value;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var returnValue = Value?.Interpret(memory);
        throw new InternalReturnException(returnValue);
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ReturnInstruction);
    public bool Equals(ReturnInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (!Value.Equals(instruction.Value)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Instructions);
}