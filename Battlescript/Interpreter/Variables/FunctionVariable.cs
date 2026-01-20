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
    
    public FunctionVariable(FunctionVariable other) : this(other.Name, other.FunctionClosure, other.Parameters, other.Instructions) { }
    
    public virtual Variable RunFunction(CallStack callStack, Closure closure, ArgumentSet arguments, Instruction? inst = null)
    {
        var lineValue = inst?.Line ?? 0;
        var expressionValue = inst?.Expression ?? "";
        callStack.AddFrame(lineValue, expressionValue, Name);
        var newClosure = new Closure(FunctionClosure, this);
        ArgumentTransfer.Execute(callStack, newClosure, arguments, Parameters);
        var returnValue = RunInstructions(callStack, newClosure);
        callStack.RemoveFrame();
        return returnValue ?? new ConstantVariable();
    }
    
    protected Variable? RunInstructions(CallStack callStack, Closure closure)
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
    
    #region Equality

    public override bool Equals(object? obj) => obj is FunctionVariable variable && Equals(variable);

    public bool Equals(FunctionVariable? other) =>
        other is not null &&
        Equals(Parameters, other.Parameters) &&
        Instructions.SequenceEqual(other.Instructions);

    public override bool Equals(Variable? other) => other is FunctionVariable variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Parameters, Instructions);

    public static bool operator ==(FunctionVariable? left, FunctionVariable? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(FunctionVariable? left, FunctionVariable? right) => !(left == right);

    #endregion
}