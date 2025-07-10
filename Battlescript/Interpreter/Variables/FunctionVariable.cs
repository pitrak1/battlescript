namespace Battlescript;

public class FunctionVariable : Variable, IEquatable<FunctionVariable>
{
    public List<Instruction> Parameters { get; set; }
    public List<Instruction> Instructions { get; set; }

    public FunctionVariable(List<Instruction>? parameters, List<Instruction>? instructions)
    {
        Parameters = parameters ?? [];
        Instructions = instructions ?? [];
    }
    
    public Variable RunFunction(Memory memory, List<Variable> arguments, ObjectVariable? objectVariable = null)
    {
        memory.AddScope();
        
        ArgumentTransfer.RunAndApply(memory, arguments, Parameters, objectVariable);
        
        var returnValue = RunInstructions(memory);
            
        memory.RemoveScope();
        
        return returnValue ?? new ConstantVariable();
    }

    public Variable RunFunction(Memory memory, List<Instruction> arguments, ObjectVariable? objectVariable = null)
    {
        memory.AddScope();
        
        ArgumentTransfer.RunAndApply(memory, arguments, Parameters, objectVariable);
        
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
        
        return Parameters.SequenceEqual(variable.Parameters) && Instructions.SequenceEqual(variable.Instructions);
    }
    
    public override int GetHashCode() => HashCode.Combine(Parameters, Instructions);
}