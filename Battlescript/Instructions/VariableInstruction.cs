namespace Battlescript;

public class VariableInstruction : Instruction
{
    public string Name { get; set; } 

    public VariableInstruction(List<Token> tokens) : base(tokens)
    {
        Name = tokens[0].Value;
        ParseNext(tokens, 1);
    }

    public VariableInstruction(
        string name, 
        Instruction? next = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Name = name;
        Next = next;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var variable = closure.GetVariable(callStack, Name);

        if (Next is null)
        {
            return variable;
        }
        else
        {
            if (variable is ObjectVariable objectVariable)
            {
                return Next.Interpret(callStack, closure, variable);
            }
            
            return Next.Interpret(callStack, closure, variable);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as VariableInstruction);
    public bool Equals(VariableInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        return Name == inst.Name;
    }
    
    public override int GetHashCode()
    {
        return Name.GetHashCode() * 10;
    }
}