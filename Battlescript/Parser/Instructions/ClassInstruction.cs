namespace Battlescript;

public class ClassInstruction : Instruction
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
        List<Variable> superclasses = new List<Variable>();
        if (Superclasses.Count > 0)
        {
            foreach (var superclassInstruction in Superclasses)
            {
                superclasses.Add(superclassInstruction.Interpret(memory));
            }
        }
        
        memory.AddScope();

        foreach (var instruction in Instructions)
        {
            instruction.Interpret(memory);
        }

        var classScope = memory.RemoveScope();
        var classVariable = new ClassVariable(classScope);
        memory.AssignToVariable(new VariableInstruction(Name), classVariable);
        return classVariable;
    }
}