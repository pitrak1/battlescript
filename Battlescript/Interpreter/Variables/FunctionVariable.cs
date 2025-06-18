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
        var classScopesAdded = AddClassScopes(memory, objectVariable);
            
        memory.AddScope();

        for (var i = 0; i < Parameters.Count; i++)
        {
            var parameter = Parameters[i];
            if (parameter is VariableInstruction variableInstruction)
            {
                if (i >= arguments.Count)
                {
                    throw new Exception("Missing a required argument at arg " + (i + 1));
                }
                
                var value = arguments[i];
                memory.SetVariable(new VariableInstruction(variableInstruction.Name), value);
            } else if (parameter is AssignmentInstruction assignmentInstruction)
            {
                if (i < arguments.Count)
                {
                    var value = arguments[i];
                    memory.SetVariable((VariableInstruction)assignmentInstruction.Left, value);
                }
                else
                {
                    var value = assignmentInstruction.Right.Interpret(memory);
                    memory.SetVariable((VariableInstruction)assignmentInstruction.Left, value);
                }
            }
        }

        var returnValue = RunInstructions(memory);
            
        memory.RemoveScopes(classScopesAdded + 1);
        
        return returnValue ?? new NoneVariable();
    }
    
    public Variable RunFunction(Memory memory, List<Instruction> arguments, ObjectVariable? objectVariable = null)
    {
        var variableArguments = new List<Variable>();
        foreach (var instruction in arguments)
        {
            variableArguments.Add(instruction.Interpret(memory, objectVariable));
        }
        
        return RunFunction(memory, variableArguments);
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