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

        ProcessPositionalArguments(arguments, parameters, result, argsSequence, handledNames);
        ProcessKeywordArguments(arguments, parameters, result, kwargsMapping, handledNames);
        ApplyDefaultValues(callStack, closure, parameters, result, handledNames);
        ValidateAllParametersHandled(parameters, handledNames);
        AddSpecialParametersToResult(parameters, result, argsSequence, kwargsMapping);

        return result;
    }

    private static void ProcessPositionalArguments(
        ArgumentSet arguments,
        ParameterSet parameters,
        Dictionary<string, Variable> result,
        SequenceVariable argsSequence,
        HashSet<string> handledNames)
    {
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
                var expected = parameters.AllParameterNames.Count;
                var given = arguments.Positionals.Count;
                throw new InternalRaiseException(BtlTypes.Types.TypeError,
                    $"takes {expected} positional argument{(expected == 1 ? "" : "s")} but {given} {(given == 1 ? "was" : "were")} given");
            }
        }
    }

    private static void ProcessKeywordArguments(
        ArgumentSet arguments,
        ParameterSet parameters,
        Dictionary<string, Variable> result,
        MappingVariable kwargsMapping,
        HashSet<string> handledNames)
    {
        foreach (var (name, value) in arguments.Keywords)
        {
            if (handledNames.Contains(name))
            {
                throw new InternalRaiseException(BtlTypes.Types.TypeError, $"got multiple values for argument '{name}'");
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
                throw new InternalRaiseException(BtlTypes.Types.TypeError, $"got an unexpected keyword argument '{name}'");
            }
        }
    }

    private static void ApplyDefaultValues(
        CallStack callStack,
        Closure closure,
        ParameterSet parameters,
        Dictionary<string, Variable> result,
        HashSet<string> handledNames)
    {
        foreach (var (name, value) in parameters.DefaultValues)
        {
            if (!result.ContainsKey(name))
            {
                result[name] = value.Interpret(callStack, closure);
                handledNames.Add(name);
            }
        }
    }

    private static void ValidateAllParametersHandled(ParameterSet parameters, HashSet<string> handledNames)
    {
        var missing = parameters.AllParameterNames.Except(handledNames).ToList();
        if (missing.Count == 0) return;

        var message = missing.Count == 1
            ? $"missing 1 required positional argument: '{missing[0]}'"
            : $"missing {missing.Count} required positional arguments: {FormatMissingArgs(missing)}";

        throw new InternalRaiseException(BtlTypes.Types.TypeError, message);
    }

    private static string FormatMissingArgs(List<string> args)
    {
        if (args.Count == 2)
            return $"'{args[0]}' and '{args[1]}'";

        var allButLast = args.Take(args.Count - 1).Select(a => $"'{a}'");
        return $"{string.Join(", ", allButLast)}, and '{args.Last()}'";
    }

    private static void AddSpecialParametersToResult(
        ParameterSet parameters,
        Dictionary<string, Variable> result,
        SequenceVariable argsSequence,
        MappingVariable kwargsMapping)
    {
        if (parameters.ArgsName is not null)
        {
            result[parameters.ArgsName] = BtlTypes.Create(BtlTypes.Types.Tuple, argsSequence);
        }

        if (parameters.KwargsName is not null)
        {
            result[parameters.KwargsName] = BtlTypes.Create(BtlTypes.Types.Dictionary, kwargsMapping);
        }
    }
}
