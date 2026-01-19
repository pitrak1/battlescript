namespace Battlescript;

public class AssertInstruction : Instruction
{
    public Instruction Condition { get; set; }
    public AssertInstruction(List<Token> tokens) : base(tokens)
    {
        var tokensAfterAssertKeyword = tokens.GetRange(1, tokens.Count - 1);
        Condition = InstructionFactory.Create(tokensAfterAssertKeyword)!;
    }

    public AssertInstruction(
        Instruction condition, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Condition = condition;
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var condition = Condition.Interpret(callStack, closure);
        
        if (!Truthiness.IsTruthy(callStack, closure, condition!, this))
        {
            throw new InternalRaiseException(BsTypes.Types.AssertionError, "");
        }

        return null;
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as AssertInstruction);
    public bool Equals(AssertInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Condition.Equals(inst.Condition);
    }
    
    public override int GetHashCode() => 93 * Condition.GetHashCode();
}