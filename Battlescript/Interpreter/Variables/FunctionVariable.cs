namespace Battlescript;

public class FunctionVariable : Variable, IEquatable<FunctionVariable>
{
    public string? Name { get; set; }
    public ParameterSet Parameters { get; set; }
    public List<Instruction> Instructions { get; set; }

    public FunctionVariable(string? name, ParameterSet? parameters = null, List<Instruction>? instructions = null)
    {
        Name = name;
        Parameters = parameters ?? new ParameterSet();
        Instructions = instructions ?? [];
    }
    
    public Variable RunFunction(Memory memory, ArgumentSet arguments, Instruction? inst = null)
    {
        var lineValue = inst?.Line ?? 0;
        var expressionValue = inst?.Expression ?? "";
        memory.AddScope(lineValue, expressionValue, Name);
        // memory.AddScope(inst.Line, inst.Expression, Name);
        arguments.ApplyToMemory(memory, Parameters);
        var returnValue = RunInstructions(memory);
        memory.RemoveScope();
        return returnValue ?? new ConstantVariable();
    }
    
    private Variable? RunInstructions(Memory memory)
    {
        try
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(memory);
            }
        }
        catch (InternalReturnException e)
        {
            return e.Value;
        }
        
        return new ConstantVariable();
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as FunctionVariable);
    public bool Equals(FunctionVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;

        if (!Parameters.Names.SequenceEqual(variable.Parameters.Names) ||
            !Parameters.DefaultValues.SequenceEqual(variable.Parameters.DefaultValues))
        {
            return false;
        }
        
        return Instructions.SequenceEqual(variable.Instructions);
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Instructions);
}