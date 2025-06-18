namespace Battlescript;

public class BreakInstruction : Instruction, IEquatable<BreakInstruction>
{
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        throw new InternalBreakException();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as BreakInstruction);
    public bool Equals(BreakInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(1);
}