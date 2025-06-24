namespace Battlescript;

public class FunctionVariable(List<Instruction>? parameters, List<Instruction>? instructions) : ReferenceVariable, IEquatable<FunctionVariable>
{
    public List<Instruction> Parameters { get; set; } = parameters ?? [];
    public List<Instruction> Instructions { get; set; } = instructions ?? [];
    
    public override bool SetItem(Memory memory, Variable valueVariable, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a function variable");
    }
    
    public override Variable? GetItem(Memory memory, SquareBracketsInstruction index, ObjectVariable? objectContext = null)
    {
        throw new Exception("Cannot index a function variable");
    }
    
    public Variable RunFunction(Memory memory, List<Variable> arguments, ObjectVariable? objectVariable = null)
    {
        memory.AddScope();
        
        ArgumentTransfer.Run(memory, arguments, Parameters, objectVariable);
        
        var returnValue = RunInstructions(memory);
            
        memory.RemoveScope();
        
        return returnValue ?? new NoneVariable();
    }

    public Variable RunFunction(Memory memory, List<Instruction> arguments, ObjectVariable? objectVariable = null)
    {
        memory.AddScope();
        
        ArgumentTransfer.Run(memory, arguments, Parameters, objectVariable);
        
        var returnValue = RunInstructions(memory);
            
        memory.RemoveScope();
        
        return returnValue ?? new NoneVariable();
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
        
        return new NoneVariable();
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