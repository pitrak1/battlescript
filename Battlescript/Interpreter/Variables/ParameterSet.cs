namespace Battlescript;

public class ParameterSet : IEquatable<ParameterSet>
{
    public List<string> Names { get; set; } = [];
    public Dictionary<string, Instruction> DefaultValues { get; set; } = [];

    public ParameterSet(List<Instruction>? paramInstructions = null)
    {
        paramInstructions ??= [];

        var inDefaultArguments = false;
        foreach (var inst in paramInstructions)
        {
            if (inst is AssignmentInstruction assignmentInstruction)
            {
                var name = ((VariableInstruction)assignmentInstruction.Left).Name;
                Names.Add(name);
                DefaultValues.Add(name, assignmentInstruction.Right);
                inDefaultArguments = true;
            }
            else if (inst is VariableInstruction variableInstruction)
            {
                if (inDefaultArguments)
                {
                    throw new InterpreterRequiredParamFollowsDefaultParamException();
                }

                Names.Add(variableInstruction.Name);
            }
        }
    }

    public Dictionary<string, Variable?> GetVariableDictionary(CallStack callStack, Closure closure)
    {
        var parameterObject = new Dictionary<string, Variable?>();

        foreach (var name in Names)
        {
            if (DefaultValues.ContainsKey(name))
            {
                parameterObject.Add(name, DefaultValues[name].Interpret(callStack, closure));
            }
            else
            {
                parameterObject.Add(name, null);
            }
        }

        return parameterObject;
    }

    #region Equality

    public override bool Equals(object? obj) => obj is ParameterSet other && Equals(other);

    public bool Equals(ParameterSet? other) =>
        other is not null &&
        Names.SequenceEqual(other.Names) &&
        DefaultValues.OrderBy(kvp => kvp.Key).SequenceEqual(other.DefaultValues.OrderBy(kvp => kvp.Key));

    public override int GetHashCode() => HashCode.Combine(Names, DefaultValues);

    public static bool operator ==(ParameterSet? left, ParameterSet? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ParameterSet? left, ParameterSet? right) => !(left == right);

    #endregion
}