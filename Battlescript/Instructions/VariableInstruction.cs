namespace Battlescript;

public class VariableInstruction : Instruction, IEquatable<VariableInstruction>
{
    public string Name { get; set; } 
    public Instruction? Next { get; set; }

    public VariableInstruction(List<Token> tokens)
    {
        Name = tokens[0].Value;
        Next = tokens.Count > 1 ? Parse(tokens.Slice(1, tokens.Count - 1)) : null;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public VariableInstruction(string name, Instruction? next = null)
    {
        Name = name;
        Next = next;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var variable = memory.GetVariable(Name);

        if (Next is null)
        {
            return variable;
        }
        else
        {
            if (variable is ObjectVariable objectVariable)
            {
                return Next.Interpret(memory, variable, objectVariable);
            }
            
            return Next.Interpret(memory, variable);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as VariableInstruction);
    public bool Equals(VariableInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Name != instruction.Name) return false;
        
        if (Next is not null && !Next.Equals(instruction.Next)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Name, Next, Instructions);
}