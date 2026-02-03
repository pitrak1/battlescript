namespace Battlescript;

public class ParameterSet : IEquatable<ParameterSet>
{
    public List<string> Positionals { get; } = [];
    public List<string> Any { get; } = [];
    public List<string> Keywords { get; } = [];
    
    public List<string> AllParameterNames { get => Positionals.Concat(Any).Concat(Keywords).ToList(); }
    public Dictionary<string, Instruction> DefaultValues { get; } = [];

    public string? ArgsName { get; private set; }
    public string? KwargsName { get; private set; }

    public ParameterSet(List<Instruction>? parameters = null)
    {
        if (parameters is null) return;

        int? posOnlyIndex = FindNullableIndex(parameters, item => item is OperationInstruction { Operation: "/" });
        int? kwOnlyIndex = FindNullableIndex(parameters, item => item is OperationInstruction { Operation: "*" });
        int? argsIndex = FindNullableIndex(parameters, item => item is SpecialVariableInstruction { Asterisks: 1 });
        int? kwargsIndex = FindNullableIndex(parameters, item => item is SpecialVariableInstruction { Asterisks: 2 });

        if (kwOnlyIndex is not null && argsIndex is not null)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "* argument may appear only once");
        }

        if (kwargsIndex is not null && kwargsIndex != parameters.Count - 1)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }

        CheckDefaultParameterPositions(parameters, posOnlyIndex, kwOnlyIndex, argsIndex);

        PopulatePosOnly(parameters, posOnlyIndex);
        PopulateAny(parameters, posOnlyIndex, kwOnlyIndex, argsIndex);
        PopulateKwOnly(parameters, kwOnlyIndex, argsIndex, kwargsIndex);

        if (argsIndex is not null && parameters[argsIndex.Value] is SpecialVariableInstruction args)
        {
            ArgsName = args.Name;
        }
        
        if (kwargsIndex is not null && parameters[kwargsIndex.Value] is SpecialVariableInstruction kwargs)
        {
            KwargsName = kwargs.Name;
        }
    }

    private void PopulatePosOnly(List<Instruction> parameters, int? posOnlyIndex)
    {
        if (posOnlyIndex is not null)
        {
            for (var i = 0; i < posOnlyIndex; i++)
            {
                AddParameterAndDefaultValue(parameters[i], Positionals);
            }
        }
    }

    private void PopulateAny(List<Instruction> parameters, int? posOnlyIndex, int? kwOnlyIndex, int? argsIndex)
    {
        var startIndex = (posOnlyIndex + 1) ?? 0;
        var stopIndex = kwOnlyIndex ?? argsIndex ?? parameters.Count;

        for (var i = startIndex; i < stopIndex; i++)
        {
            AddParameterAndDefaultValue(parameters[i], Any);
        }
    }

    private void PopulateKwOnly(List<Instruction> parameters, int? kwOnlyIndex, int? argsIndex, int? kwargsIndex)
    {
        // Keyword-only parameters come after * or *args
        int? startIndex = kwOnlyIndex ?? argsIndex;
        if (startIndex is null) return;

        var stopIndex = kwargsIndex ?? parameters.Count;
        for (var i = startIndex.Value + 1; i < stopIndex; i++)
        {
            AddParameterAndDefaultValue(parameters[i], Keywords);
        }
    }

    private void AddParameterAndDefaultValue(Instruction parameter, List<string> collection)
    {
        if (parameter is AssignmentInstruction assignmentInstruction)
        {
            var name = ((VariableInstruction)assignmentInstruction.Left).Name;
            collection.Add(name);
            DefaultValues.Add(name, assignmentInstruction.Right);
        }
        else if (parameter is VariableInstruction and not SpecialVariableInstruction)
        {
            collection.Add(((VariableInstruction)parameter).Name);
        }
    }

    private void CheckDefaultParameterPositions(List<Instruction> parameters, int? posOnlyIndex, int? kwOnlyIndex, int? argsIndex)
    {
        bool inDefaultParameters = false;
        var startIndex = (posOnlyIndex + 1) ?? 0;
        var stopIndex = kwOnlyIndex ?? argsIndex ?? parameters.Count;
        for (var i = startIndex; i < stopIndex; i++)
        {
            if (parameters[i] is VariableInstruction and not SpecialVariableInstruction && inDefaultParameters)
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "non-default argument follows default argument");
            }
            else if (parameters[i] is AssignmentInstruction)
            {
                inDefaultParameters = true;
            }
        }
    }

    private static int? FindNullableIndex(List<Instruction> parameters, Func<Instruction, bool> predicate)
    {
        var index = parameters.FindIndex(i => predicate(i));
        return index >= 0 ? index : null;
    }

    public string? GetPositionalByIndex(int index)
    {
        if (index < Positionals.Count)
        {
            return Positionals[index];
        }
        else if (index < Positionals.Count + Any.Count)
        {
            return Any[index];
        }
        return null;
    }

    public bool IsValidKeywordArg(string name)
    {
        return Any.Contains(name) || Keywords.Contains(name);
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is ParameterSet other && Equals(other);

    public bool Equals(ParameterSet? other) =>
        other is not null &&
        Positionals.SequenceEqual(other.Positionals) &&
        Any.SequenceEqual(other.Any) &&
        Keywords.SequenceEqual(other.Keywords) &&
        DefaultValues.OrderBy(kvp => kvp.Key).SequenceEqual(other.DefaultValues.OrderBy(kvp => kvp.Key)) &&
        ArgsName == other.ArgsName &&
        KwargsName == other.KwargsName;

    public override int GetHashCode() => HashCode.Combine(Positionals, Any, Keywords, DefaultValues, ArgsName, KwargsName);

    public static bool operator ==(ParameterSet? left, ParameterSet? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ParameterSet? left, ParameterSet? right) => !(left == right);

    #endregion
}