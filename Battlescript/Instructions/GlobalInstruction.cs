using System.Diagnostics;

namespace Battlescript;

public class GlobalInstruction : Instruction
{
    public string Name { get; set; } 

    public GlobalInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[1].Value;
    }

    public GlobalInstruction(string name, int? line = null, string? expression = null) : base(line, expression)
    {
        Name = name;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        closure.CreateGlobalReference(Name);
        return null;
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as GlobalInstruction);
    public bool Equals(GlobalInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Name == inst.Name;
    }
    
    public override int GetHashCode() => Name.GetHashCode() * 25;
}