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
        var result = new Dictionary<string, Variable>();
        var usedKeywords = new HashSet<string>();

        for (var i = 0; i < parameters.Names.Count; i++)
        {
            var parameterName = parameters.Names[i];
            var hasPositionalArg = i < arguments.Positionals.Count;
            var hasKeywordArg = arguments.Keywords.ContainsKey(parameterName);
            var hasDefaultValue = parameters.DefaultValues.ContainsKey(parameterName);

            if (hasPositionalArg && hasKeywordArg)
            {
                throw new InterpreterMultipleArgumentsForParameterException(parameterName);
            } else if (hasPositionalArg)
            {
                result[parameterName] = arguments.Positionals[i];
            }
            else if (hasKeywordArg)
            {
                result[parameterName] = arguments.Keywords[parameterName];
                usedKeywords.Add(parameterName);
            }
            else if (hasDefaultValue)
            {
                result[parameterName] = parameters.DefaultValues[parameterName].Interpret(callStack, closure);
            }
            else
            {
                throw new InterpreterMissingRequiredArgumentException(parameterName);
            }
        }

        var extraPositionalCount = arguments.Positionals.Count - parameters.Names.Count;
        if (extraPositionalCount > 0)
        {
            throw new InterpreterUnknownPositionalArgumentException(extraPositionalCount);
        }

        var unusedKeywords = arguments.Keywords.Keys.Except(usedKeywords).ToList();
        if (unusedKeywords.Count > 0)
        {
            throw new InterpreterUnknownKeywordArgumentException(unusedKeywords);
        }

        return result;
    }
}
