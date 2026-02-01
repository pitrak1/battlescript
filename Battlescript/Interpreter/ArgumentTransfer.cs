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
        var argsSequence = new SequenceVariable();
        var kwargsMapping = new MappingVariable();
        var handledNames = new HashSet<string>();
        
        for (var i = 0; i < arguments.Positionals.Count; i++)
        {
            var argument = arguments.Positionals[i];
            var parameter = parameters.GetPositionalByIndex(i);

            if (parameter is not null)
            {
                result[parameter] = argument;
                handledNames.Add(parameter);
            }
            else if (parameters.ArgsName is not null)
            {
                argsSequence.Values.Add(argument);
            }
            else
            {
                throw new InternalRaiseException(BtlTypes.Types.TypeError, "too many positional arguments, fix later");
            }
        }

        foreach (var (name, value) in arguments.Keywords)
        {
            if (handledNames.Contains(name))
            {
                throw new InternalRaiseException(BtlTypes.Types.TypeError, $"multiple values for {name}, fix later");
            }

            if (parameters.IsValidKeywordArg(name))
            {
                result[name] = value;
                handledNames.Add(name);
            }
            else if (parameters.KwargsName is not null)
            {
                kwargsMapping.StringValues.Add(name, value);
            }
            else
            {
                throw new InternalRaiseException(BtlTypes.Types.TypeError, $"unknown keyword {name}, fix later");
            }
        }

        foreach (var (name, value) in parameters.DefaultValues)
        {
            if (!result.ContainsKey(name))
            {
                result[name] = value.Interpret(callStack, closure);
                handledNames.Add(name);
            }
        }
        
        var missing = parameters.AllParameterNames.Except(handledNames);
        if (missing.Any())
        {
            throw new InternalRaiseException(BtlTypes.Types.TypeError, $"missing parameters {string.Join(", ", missing)}, fix later");
        }

        if (parameters.ArgsName is not null)
        {
            result[parameters.ArgsName] = BtlTypes.Create(BtlTypes.Types.List, argsSequence);
        }

        if (parameters.KwargsName is not null)
        {
            result[parameters.KwargsName] = BtlTypes.Create(BtlTypes.Types.Dictionary, kwargsMapping);
        }

        return result;
    }
}
