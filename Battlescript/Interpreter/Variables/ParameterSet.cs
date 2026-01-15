namespace Battlescript;

public class ParameterSet
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
            } else if (inst is VariableInstruction variableInstruction)
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
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ParameterSet);
    public bool Equals(ParameterSet? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        
        var namesEqual = Names.SequenceEqual(other.Names);
        var defaultValuesEqual = DefaultValues.OrderBy(kvp => kvp.Key).SequenceEqual(other.DefaultValues.OrderBy(kvp => kvp.Key));
        
        return namesEqual && defaultValuesEqual;
    }
    
    public override int GetHashCode()
    {
        int hash = 25;

        for (int i = 0; i < Names.Count; i++)
        {
            hash += Names[i].GetHashCode() * 20 * (i + 1);
        }

        for (int i = 0; i < DefaultValues.Keys.Count; i++)
        {
            var key = DefaultValues.Keys.ElementAt(i);
            var value = DefaultValues[key];
            hash += key.GetHashCode() * 30 * (i + 1) + value.GetHashCode() * 40;
        }

        return hash;
    }
}