namespace Battlescript;

public class ClassInstruction : Instruction, IEquatable<ClassInstruction>
{
    public string Name { get; set; } 
    public List<Instruction> Superclasses { get; set; }

    public ClassInstruction(List<Token> tokens)
    {
        List<Instruction> superClasses = [];
        if (tokens.Count > 3)
        {
            var tokensInParens = tokens.GetRange(2, tokens.Count - 3);
            superClasses = ParseAndRunEntriesWithinSeparator(tokensInParens, [","]).Values;
        }

        Name = tokens[1].Value;
        Superclasses = superClasses;
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    public ClassInstruction(string name, List<Instruction>? superclasses = null)
    {
        Name = name;
        Superclasses = superclasses ?? [];
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? context = null, 
        Variable? objectContext = null)
    {
        List<ClassVariable> superclasses = new List<ClassVariable>();
        if (Superclasses.Count > 0)
        {
            foreach (var superclassInstruction in Superclasses)
            {
                superclasses.Add(superclassInstruction.Interpret(memory) as ClassVariable);
            }
        }
        
        memory.AddScope();

        foreach (var instruction in Instructions)
        {
            instruction.Interpret(memory);
        }

        var classScope = memory.RemoveScope();
        var classVariable = new ClassVariable(classScope, superclasses);
        memory.SetVariable(new VariableInstruction(Name), classVariable);
        return classVariable;
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ClassInstruction);
    public bool Equals(ClassInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (!Superclasses.SequenceEqual(instruction.Superclasses) || Name != instruction.Name) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Name, Superclasses, Instructions);
    public static bool operator ==(ClassInstruction left, ClassInstruction right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(ClassInstruction left, ClassInstruction right) => !(left == right);
}