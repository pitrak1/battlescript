namespace Battlescript;

public class ContinueInstruction() : Instruction([])
{
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        throw new InternalContinueException();
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ContinueInstruction);
    public bool Equals(ContinueInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;

        return true;
    }
    
    public override int GetHashCode()
    {
        return 66;
    }
}