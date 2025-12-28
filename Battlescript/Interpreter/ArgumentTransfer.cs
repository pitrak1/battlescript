namespace Battlescript;

public static class ArgumentTransfer
{
    public static void Execute(CallStack callStack, Closure closure, ArgumentSet arguments, ParameterSet parameters)
    {
        var variableTransferDictionary = GetVariableTransferDictionary(callStack, closure, arguments, parameters);
        
        foreach (var (name, value) in variableTransferDictionary)
        {
            closure.SetVariable(callStack, new VariableInstruction(name: name), value);
        }
    }

    public static Dictionary<string, Variable> GetVariableTransferDictionary(
        CallStack callStack,
        Closure closure,
        ArgumentSet arguments, 
        ParameterSet parameters)
    {
        var parameterDictionary = parameters.GetVariableDictionary(callStack, closure);
        var positionalArgumentDictionary = arguments.GetPositionalArgumentsAsDictionary();

        for (var i = 0; i < parameters.Names.Count; i++)
        {
            var parameterName = parameters.Names[i];
            var argumentIsProvidedByPosition = positionalArgumentDictionary.ContainsKey(i);
            var argumentIsProvidedByKeyword = arguments.Keywords.ContainsKey(parameterName);
            var parameterHasDefaultValue = parameters.DefaultValues.ContainsKey(parameterName);
            
            if (argumentIsProvidedByPosition && argumentIsProvidedByKeyword)
            {
                throw new InterpreterMultipleArgumentsForParameterException(parameterName);
            }
            else if (argumentIsProvidedByPosition)
            {
                parameterDictionary[parameterName] = positionalArgumentDictionary[i];
                positionalArgumentDictionary.Remove(i);
            }
            else if (argumentIsProvidedByKeyword)
            {
                parameterDictionary[parameterName] = arguments.Keywords[parameterName];
                arguments.Keywords.Remove(parameterName);
            }
            else if (!parameterHasDefaultValue)
            {
                throw new InterpreterMissingRequiredArgumentException(parameterName);
            }
        }
        
        if (positionalArgumentDictionary.Count > 0)
        {
            throw new Exception("unknown positional arguments at " + positionalArgumentDictionary.Keys.ToList());
        }
        
        if (arguments.Keywords.Count > 0)
        {
            throw new Exception("unknown keyword arguments at " + arguments.Keywords.Keys.ToList());
        }
        
        return parameterDictionary;
    }
}