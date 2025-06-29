namespace Battlescript;

public class ClassInstruction : Instruction, IEquatable<ClassInstruction>
{
    public string Name { get; set; } 
    public List<Instruction> Superclasses { get; set; }

    public ClassInstruction(List<Token> tokens)
    {
        List<Instruction> superClasses = [];
        
        // class name(): would be five tokens, so if it's more than 5, we know there are superclasses
        if (tokens.Count > 5)
        {
            var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
            superClasses = InstructionUtilities.ParseEntriesBetweenSeparatingCharacters(tokensInParens, [","])!;
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
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
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
}