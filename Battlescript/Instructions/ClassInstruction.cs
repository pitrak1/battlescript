namespace Battlescript;

public class ClassInstruction : Instruction
{
    public string Name { get; set; } 
    public List<Instruction> Superclasses { get; set; }

    public ClassInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        
        Name = tokens[1].Value;
        Superclasses = ParseSuperclasses(tokens);
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
    }

    private List<Instruction> ParseSuperclasses(List<Token> tokens)
    {
        // class name(): would be five tokens, so if it's more than 5, we know there are superclasses
        if (tokens.Count > 5)
        {
            var tokensInParens = tokens.GetRange(3, tokens.Count - 5);
            return InstructionUtilities.ParseEntriesBetweenDelimiters(tokensInParens, [","])!;
        }
        else
        {
            return [];
        }
    }

    public ClassInstruction(
        string name, 
        List<Instruction>? superclasses = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Name = name;
        Superclasses = superclasses ?? [];
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        List<ClassVariable> superclasses = new List<ClassVariable>();
        if (Superclasses.Count > 0)
        {
            foreach (var superclassInstruction in Superclasses)
            {
                var superclass = superclassInstruction.Interpret(callStack, closure);
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
        
        callStack.AddFrame(Line, Expression, Name);
        var classVariable = new ClassVariable(Name, [], closure, superclasses);
        var newClosure = new Closure(closure, classVariable);

        foreach (var instruction in Instructions)
        {
            instruction.Interpret(callStack, newClosure);
        }

        callStack.RemoveFrame();
        var values = newClosure.Scopes[^1].Values.ToDictionary();
        classVariable.Values = values;
        closure.SetVariable(callStack, new VariableInstruction(Name), classVariable);
        return classVariable;
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ClassInstruction);
    public bool Equals(ClassInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var superclassesEqual = Superclasses.SequenceEqual(inst.Superclasses);
        var instructionsEqual = Instructions.SequenceEqual(inst.Instructions);
        return Name == inst.Name && superclassesEqual && instructionsEqual;
    }
    
    public override int GetHashCode()
    {
        int hash = 17;

        for (int i = 0; i < Superclasses.Count; i++)
        {
            hash += Superclasses[i].GetHashCode() * 73 * (i + 1);
        }
        
        for (int i = 0; i < Instructions.Count; i++)
        {
            hash += Instructions[i].GetHashCode() * 40 * (i + 1);
        }

        hash += Name.GetHashCode() * 37;
        return hash;
    }
}