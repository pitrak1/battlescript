namespace Battlescript;

public class ClassInstruction : Instruction
{
    public string Name { get; set; } 
    public List<Instruction> Superclasses { get; set; }

    public ClassInstruction(List<Token> tokens) : base(tokens)
    {
        List<Instruction> superClasses = [];
        
        // class name(): would be five tokens, so if it's more than 5, we know there are superclasses
        if (tokens.Count > 5)
        {
            var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
            superClasses = InstructionUtilities.ParseEntriesBetweenDelimiters(tokensInParens, [","])!;
        }

        Name = tokens[1].Value;
        Superclasses = superClasses;
    }

    public ClassInstruction(string name, List<Instruction>? superclasses = null) : base([])
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
                var superclass = superclassInstruction.Interpret(memory);
                if (superclass is ClassVariable superclassVariable)
                {
                    superclasses.Add(superclassVariable);
                }
                else
                {
                    throw new Exception($"Superclass {superclass} is not a class");
                }
            }
        }
        
        // There's an issue here that if we assign to a variable in the class definition using SetVariable, it may assign
        // to a scope outside the scope of the class that already exists in a lower scope.  We may need to create a new
        // memory instance to run it in there, but if we do that, we may lose refernences we need to define the class.
        // I'll have to think about this.
        memory.AddScope(new MemoryScope(FileName, Line, Name, Expression));

        foreach (var instruction in Instructions)
        {
            instruction.Interpret(memory);
        }

        var classScope = memory.RemoveScope();
        var classVariable = new ClassVariable(Name, classScope.Variables, superclasses);
        memory.SetVariable(new VariableInstruction(Name), classVariable);
        return classVariable;
    }
}