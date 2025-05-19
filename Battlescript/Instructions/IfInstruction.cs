namespace Battlescript;

public class IfInstruction : Instruction, IEquatable<IfInstruction>
{
    public Instruction Condition { get; set; }

    public IfInstruction(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("If statement should end with colon", tokens[0]);
        }

        Condition = Parse(tokens.GetRange(1, tokens.Count - 2));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public IfInstruction(Instruction condition, List<Instruction>? instructions = null)
    {
        Condition = condition;
        Instructions = instructions ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        var condition = Condition.Interpret(memory);
        if (InterpreterUtilities.IsVariableTruthy(condition))
        {
            memory.AddScope();
            foreach (var inst in Instructions)
            {
                inst.Interpret(memory);
            }
            memory.RemoveScope();
        }

        return new NullVariable();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as IfInstruction);
    public bool Equals(IfInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (Condition != instruction.Condition) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Condition, Instructions);
    public static bool operator ==(IfInstruction left, IfInstruction right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(IfInstruction left, IfInstruction right) => !(left == right);
}