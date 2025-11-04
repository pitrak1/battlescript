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
}