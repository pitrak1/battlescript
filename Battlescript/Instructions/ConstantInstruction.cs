namespace Battlescript;

public class ConstantInstruction : Instruction
{
    public string Value { get; set; }

    public ConstantInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[0].Value;
    }

    public ConstantInstruction(string value, int? line = null, string? expression = null) : base(line, expression)
    {
        Value = value;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        switch (Value)
        {
            case "True":
                return BsTypes.True;
            case "False":
                return BsTypes.False;
            default:
                return BsTypes.None;
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ConstantInstruction);
    public bool Equals(ConstantInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Value == inst.Value;
    }
    
    public override int GetHashCode() => Value.GetHashCode() * 93;
}