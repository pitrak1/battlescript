using System.Diagnostics;

namespace Battlescript;

public class ReturnInstruction : Instruction
{
    public Instruction? Value { get; set; }

    public ReturnInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 1));
        }
    }

    public ReturnInstruction(
        Instruction? value, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Value = value;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var returnValue = Value?.Interpret(callStack, closure);
        throw new InternalReturnException(returnValue);
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ReturnInstruction);
    public bool Equals(ReturnInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Equals(Value, inst.Value);
    }
    
    public override int GetHashCode()
    {
        return Value?.GetHashCode() * 81 ?? 36;
    }
}