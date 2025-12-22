namespace Battlescript;

public class ClassInstruction : Instruction
{
    public string Name { get; set; } 
    public List<Instruction> Superclasses { get; set; }

    public ClassInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[^1].Value != ":")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
        }
        
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

    public ClassInstruction(
        string name, 
        List<Instruction>? superclasses = null, 
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Name = name;
        Superclasses = superclasses ?? [];
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
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
}