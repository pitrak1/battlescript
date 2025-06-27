namespace Battlescript;

public class NoneInstruction : Instruction, IEquatable<NoneInstruction>
{
    public override Variable Interpret(
        Memory memory,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return new ConstantVariable();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as NoneInstruction);
    public bool Equals(NoneInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;

        return GetType() == instruction.GetType();
    }
    
    public override int GetHashCode() => HashCode.Combine(1);
}