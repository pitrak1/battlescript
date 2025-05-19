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
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var variable = memory.GetVariable(Name);

        var currentObjectContext = variable is ObjectVariable objectVariable ? objectVariable : null; 
        
        // This doesn't work because we're currently getting the variable including indexes from memory in GetVariable
        // but even if we just interpreted Parens here, we would still lose the context of the object we were workign with
        if (Next is not SquareBracketsInstruction && Next is not null)
        {
            return Next.Interpret(memory, variable, currentObjectContext);
        }
        else
        {
            return variable;
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as VariableInstruction);
    public bool Equals(VariableInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;

        if (Name != instruction.Name || Next != instruction.Next) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Name, Next, Instructions);
    public static bool operator ==(VariableInstruction left, VariableInstruction right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(VariableInstruction left, VariableInstruction right) => !(left == right);
}