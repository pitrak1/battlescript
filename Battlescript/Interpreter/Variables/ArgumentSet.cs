namespace Battlescript;

public class ArgumentSet
{
    public List<Variable> Positionals { get; set; } = [];
    public Dictionary<string, Variable> Keywords { get; set; } = [];

    public ArgumentSet(CallStack callStack, Closure closure, List<Instruction> arguments, ObjectVariable? selfObject = null)
    {
        if (selfObject is not null)
        {
            Positionals.Add(selfObject);
        }

        foreach (var argument in arguments)
        {
            if (argument is AssignmentInstruction assignmentInstruction)
            {
                var keywordName = ((VariableInstruction)assignmentInstruction.Left).Name;
                Keywords[keywordName] = assignmentInstruction.Right.Interpret(callStack, closure);
            }
            else
            {
                if (Keywords.Count > 0)
                {
                    throw new InterpreterKeywordArgBeforePositionalArgException();
                }
                
                var value = argument.Interpret(callStack, closure);
                Positionals.Add(value);
            }
        }
    }

    public ArgumentSet(List<Variable> arguments)
    {
        // If we're given a list of variables instead of instructions, just treat them as positional arguments
        Positionals = arguments;
    }
    
    public Dictionary<int, Variable> GetPositionalArgumentsAsDictionary()
    {
        var result = new Dictionary<int, Variable>();
            
        for (var i = 0; i < Positionals.Count; i++)
        {
            result.Add(i, Positionals[i]);
        }
            
        return result;
    }
}