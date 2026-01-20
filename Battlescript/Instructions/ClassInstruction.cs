namespace Battlescript;

public class ClassInstruction : Instruction, IEquatable<ClassInstruction>
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
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
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
        List<ClassVariable> superclasses = InterpretSuperclasses(callStack, closure);
        
        var classVariable = new ClassVariable(Name, [], closure, superclasses);
        classVariable.Values = InterpretClassInstructionsInNewClosureScope(callStack, closure, classVariable);
        closure.SetVariable(callStack, new VariableInstruction(Name), classVariable);
        return classVariable;
    }

    private List<ClassVariable> InterpretSuperclasses(CallStack callStack, Closure closure)
    {
        return Superclasses.Select(superclassInstruction =>
        {
            var superclassVariable = superclassInstruction.Interpret(callStack, closure);
            if (superclassVariable is ClassVariable classVariable)
            {
                return classVariable;
            }
            else
            {
                throw new Exception($"Superclass {superclassVariable} is not a class");
            }
        }).ToList();
    }

    private Dictionary<string, Variable> InterpretClassInstructionsInNewClosureScope(
        CallStack callStack, Closure closure, ClassVariable classVariable)
    {
        callStack.AddFrame(Line, Expression, Name);
        var newClosure = new Closure(closure, classVariable);
        
        foreach (var instruction in Instructions)
        {
            instruction.Interpret(callStack, newClosure);
        }
        
        callStack.RemoveFrame();
        return newClosure.Scopes[^1].Values.ToDictionary();
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is ClassInstruction inst && Equals(inst);

    public bool Equals(ClassInstruction? other) =>
        other is not null &&
        Name == other.Name &&
        Superclasses.SequenceEqual(other.Superclasses) &&
        Instructions.SequenceEqual(other.Instructions);

    public override int GetHashCode() => HashCode.Combine(Superclasses, Instructions, Name);

    public static bool operator ==(ClassInstruction? left, ClassInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ClassInstruction? left, ClassInstruction? right) => !(left == right);

    #endregion
}