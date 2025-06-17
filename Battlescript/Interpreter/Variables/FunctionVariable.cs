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
    
    public Variable RunFunction(Memory memory, List<Instruction> arguments, ObjectVariable? objectVariable = null)
    {
        var classScopesAdded = AddClassScopes(memory, objectVariable);
            
        memory.AddScope();
            
        for (var i = 0; i < arguments.Count; i++)
        {
            if (Parameters[i] is VariableInstruction parameter)
            {
                var value = arguments[i].Interpret(memory);
                memory.SetVariable(new VariableInstruction(parameter.Name), value);
            }
        }

        var returnValue = RunInstructions(memory);
            
        memory.RemoveScopes(classScopesAdded + 1);
        
        return returnValue ?? new NoneVariable();
    }

    public Variable RunFunction(Memory memory, List<Variable> arguments, ObjectVariable? objectVariable = null)
    {
        var classScopesAdded = AddClassScopes(memory, objectVariable);
            
        memory.AddScope();
            
        for (var i = 0; i < arguments.Count; i++)
        {
            if (Parameters[i] is VariableInstruction parameter)
            {
                memory.SetVariable(new VariableInstruction(parameter.Name), arguments[i]);
            }
        }

        var returnValue = RunInstructions(memory);
            
        memory.RemoveScopes(classScopesAdded + 1);
        
        return returnValue ?? new NoneVariable();
    }

    private int AddClassScopes(Memory memory, ObjectVariable? objectVariable)
    {
        if (objectVariable is not null)
        {
            var classScopes = objectVariable.ClassVariable.AddClassToMemoryScopes(memory);
            memory.AddScope(objectVariable.Values);
            return classScopes + 1;
        }
        else
        {
            return 0;
        }
    }

    private Variable? RunInstructions(Memory memory)
    {
        foreach (var inst in Instructions)
        {
            inst.Interpret(memory);
        }
            
        return memory.GetVariable("return");
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