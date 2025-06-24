namespace Battlescript;

public static class ArgumentTransfer
{
    public static void Run(
        Memory memory, 
        List<Variable> arguments, 
        List<Instruction> parameters,
        ObjectVariable? objectVariable = null)
    {
        var positionalArguments = new Dictionary<int, Variable>();

        var index = 0;
        if (objectVariable is not null)
        {
            positionalArguments.Add(index, objectVariable);
            index++;
        }

        foreach (var argument in arguments)
        {
            positionalArguments.Add(index, argument);
            index++;
        }
        
        var parameterDefaultValues = GetParameterDefaultValues(memory, parameters, objectVariable);
        ApplyArgumentsToParameters(memory, positionalArguments, new Dictionary<string, Variable>(), parameters, parameterDefaultValues);

    }
    
    public static void Run(
        Memory memory, 
        List<Instruction> arguments,
        List<Instruction> parameters, 
        ObjectVariable? objectVariable = null)
    {
        var (positionalArguments, keywordArguments) = ParseArguments(memory, arguments, objectVariable);
        var parameterDefaultValues = GetParameterDefaultValues(memory, parameters, objectVariable);
        ApplyArgumentsToParameters(memory, positionalArguments, keywordArguments, parameters, parameterDefaultValues);
    }

    private static (Dictionary<int, Variable>, Dictionary<string, Variable>)
        ParseArguments(
            Memory memory,
            List<Instruction> arguments,
            ObjectVariable? objectVariable = null)
    {
        var positionalArguments = new Dictionary<int, Variable>();
        var keywordArguments = new Dictionary<string, Variable>();

        if (objectVariable is not null)
        {
            positionalArguments.Add(0, objectVariable);
        }

        for (var i = 0; i < arguments.Count; i++)
        {
            var argument = arguments[i];
            if (argument is AssignmentInstruction assignmentInstruction)
            {
                var keywordName = ((VariableInstruction)assignmentInstruction.Left).Name;
                keywordArguments[keywordName] = assignmentInstruction.Right.Interpret(memory, objectVariable);
            }
            else
            {
                if (keywordArguments.Count > 0)
                {
                    throw new InterpreterKeywordArgBeforePositionalArgException();
                }

                var value = argument.Interpret(memory, objectVariable);
                if (objectVariable is not null)
                {
                    positionalArguments[i + 1] = value;
                }
                else
                {
                    positionalArguments[i] = value;
                }
            }
        }
        
        return (positionalArguments, keywordArguments);
    }
    
    private static Dictionary<string, Variable> GetParameterDefaultValues(Memory memory, List<Instruction> parameters, ObjectVariable? objectContext = null)
    {
        var parameterDefaultValues = new Dictionary<string, Variable>();
        var hasDefaultValues = false;
        foreach (var param in parameters)
        {
            if (param is AssignmentInstruction assignmentParam)
            {
                hasDefaultValues = true;
                var parameterDefaultValue = assignmentParam.Right.Interpret(memory, objectContext);
                parameterDefaultValues.Add(((VariableInstruction)assignmentParam.Left).Name, parameterDefaultValue);
            }
            else if (hasDefaultValues)
            {
                throw new InterpreterRequiredParamFollowsDefaultParamException();
            }
        }
        
        return parameterDefaultValues;
    }

    private static void ApplyArgumentsToParameters(
        Memory memory, 
        Dictionary<int, Variable> positionalArguments,
        Dictionary<string, Variable> keywordArguments, 
        List<Instruction> parameters,
        Dictionary<string, Variable> defaultValues)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            
            string parameterName;
            if (parameter is AssignmentInstruction assignmentParam)
            {
                parameterName = ((VariableInstruction)assignmentParam.Left).Name;
            }
            else
            {
                parameterName = ((VariableInstruction)parameter).Name;
            }
            
            
            if (positionalArguments.ContainsKey(i) && keywordArguments.ContainsKey(parameterName))
            {
                throw new InterpreterMultipleArgumentsForParameterException(parameterName);
            } else if (positionalArguments.ContainsKey(i))
            {
                memory.SetVariable(new VariableInstruction(parameterName), positionalArguments[i]);
                positionalArguments.Remove(i);
            } else if (keywordArguments.ContainsKey(parameterName))
            {
                memory.SetVariable(new VariableInstruction(parameterName), keywordArguments[parameterName]);
                keywordArguments.Remove(parameterName);
            } else if (defaultValues.TryGetValue(parameterName, out var defaultValue))
            {
                memory.SetVariable(new VariableInstruction(parameterName), defaultValue);
            }
            else
            {
                throw new InterpreterMissingRequiredArgumentException(parameterName);
            }
        }
        
        if (positionalArguments.Count > 0)
        {
            throw new Exception("unknown positional arguments at " + positionalArguments.Keys.ToList());
        }

        if (keywordArguments.Count > 0)
        {
            throw new Exception("unknown keyword arguments at " + keywordArguments.Keys.ToList());
        }
    }
}