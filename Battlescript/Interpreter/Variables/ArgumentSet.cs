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

    public void ApplyToMemory(CallStack callStack, Closure closure, ParameterSet parameters)
    {
        var variableDictionary = GetVariableDictionary(callStack, closure, parameters);
        foreach (var (name, value) in variableDictionary)
        {
            closure.SetVariable(callStack, new VariableInstruction(name), value);
        }
    }

    public Dictionary<string, Variable> GetVariableDictionary(CallStack callStack, Closure closure, ParameterSet parameters)
    {
        var variableDictionary = parameters.GetVariableDictionary(callStack, closure);
        
        var positionalArguments = GetPositionalArgumentsAsDictionary();

        for (var i = 0; i < parameters.Names.Count; i++)
        {
            var parameterName = parameters.Names[i];

            if (positionalArguments.ContainsKey(i) && Keywords.ContainsKey(parameterName))
            {
                throw new InterpreterMultipleArgumentsForParameterException(parameterName);
            }
            else if (positionalArguments.ContainsKey(i))
            {
                variableDictionary[parameterName] = positionalArguments[i];
                positionalArguments.Remove(i);
            }
            else if (Keywords.ContainsKey(parameterName))
            {
                variableDictionary[parameterName] = Keywords[parameterName];
                Keywords.Remove(parameterName);
            }
        }
        
        if (positionalArguments.Count > 0)
        {
            throw new Exception("unknown positional arguments at " + positionalArguments.Keys.ToList());
        }
        
        if (Keywords.Count > 0)
        {
            throw new Exception("unknown keyword arguments at " + Keywords.Keys.ToList());
        }

        return CheckAllArgumentsPresent(variableDictionary);
    }
    
    
    private Dictionary<int, Variable> GetPositionalArgumentsAsDictionary()
    {
        var result = new Dictionary<int, Variable>();
            
        for (var i = 0; i < Positionals.Count; i++)
        {
            result.Add(i, Positionals[i]);
        }
            
        return result;
    }

    private Dictionary<string, Variable> CheckAllArgumentsPresent(Dictionary<string, Variable?> variableDictionary)
    {
        var result = new Dictionary<string, Variable>();
        foreach (var (name, value) in variableDictionary)
        {
            if (value is Variable valueVariable)
            {
                result.Add(name, valueVariable);
            }
            else
            {
                throw new InterpreterMissingRequiredArgumentException(name);
            }
        }
        return result;
    }
}