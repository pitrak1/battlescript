namespace Battlescript;

public class ContinueInstruction : Instruction, IEquatable<ContinueInstruction>
{
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        throw new InternalContinueException();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ContinueInstruction);
    public bool Equals(ContinueInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(1);
}