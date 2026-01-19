namespace Battlescript;

public class MemberInstruction : Instruction
{
    public string Value { get; set; }

    public MemberInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[1].Value;
        ParseNext(tokens, 2);
    }

    public MemberInstruction(
        string value, 
        Instruction? next = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Value = value;
        Next = next;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        return instructionContext.GetMember(callStack, closure, this, instructionContext as ObjectVariable);
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as MemberInstruction);
    public bool Equals(MemberInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Value == inst.Value && Equals(Next, inst.Next);
    }
    
    public override int GetHashCode() => HashCode.Combine(Value, Next);
}