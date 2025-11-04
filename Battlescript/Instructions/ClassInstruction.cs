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

    public ClassInstruction(string name, List<Instruction>? superclasses = null) : base([])
    {
        Name = name;
        Superclasses = superclasses ?? [];
    }

    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        List<ClassVariable> superclasses = new List<ClassVariable>();
        if (Superclasses.Count > 0)
        {
            foreach (var superclassInstruction in Superclasses)
            {
                var superclass = superclassInstruction.Interpret(callStack);
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
        
        callStack.AddScope(Line, Expression, Name);

        foreach (var instruction in Instructions)
        {
            instruction.Interpret(callStack);
        }

        var classScope = callStack.RemoveScope();
        var classVariable = new ClassVariable(Name, classScope.Values, superclasses);
        callStack.SetVariable(new VariableInstruction(Name), classVariable);
        return classVariable;
    }
}