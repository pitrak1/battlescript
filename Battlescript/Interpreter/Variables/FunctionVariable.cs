namespace Battlescript;

public class FunctionVariable : Variable, IEquatable<FunctionVariable>
{
    public string? Name { get; set; }
    public ParameterSet Parameters { get; set; }
    public List<Instruction> Instructions { get; set; }
    public Closure FunctionClosure { get; set; }

    public FunctionVariable(string? name, Closure closure, ParameterSet? parameters = null, List<Instruction>? instructions = null)
    {
        Name = name;
        FunctionClosure = closure;
        Parameters = parameters ?? new ParameterSet();
        Instructions = instructions ?? [];
    }
    
    public Variable RunFunction(CallStack callStack, Closure closure, ArgumentSet arguments, Instruction? inst = null)
    {
        var lineValue = inst?.Line ?? 0;
        var expressionValue = inst?.Expression ?? "";
        callStack.AddFrame(lineValue, expressionValue, Name);
        var newClosure = new Closure(FunctionClosure);
        // callStack.AddFrame(inst.Line, inst.Expression, Name);
        arguments.ApplyToMemory(callStack, newClosure, Parameters);
        var returnValue = RunInstructions(callStack, newClosure);
        callStack.RemoveFrame();
        return returnValue ?? new ConstantVariable();
    }
    
    private Variable? RunInstructions(CallStack callStack, Closure closure)
    {
        try
        {
            foreach (var inst in Instructions)
            {
                inst.Interpret(callStack, closure);
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