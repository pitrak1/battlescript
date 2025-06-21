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
    
    private Variable RunFunctionWithArgumentsSorted(
        Memory memory, 
        Dictionary<int, Variable> positionalArguments, 
        Dictionary<string, Variable> keywordArguments, 
        ObjectVariable? objectVariable = null)
    {
        var classScopesAdded = AddClassScopes(memory, objectVariable);
            
        memory.AddScope();

        for (var i = 0; i < Parameters.Count; i++)
        {
            var parameter = Parameters[i];
            var (parameterName, defaultValue) = GetParameterNameAndDefaultValue(memory, parameter, objectVariable);
            
            if (positionalArguments.ContainsKey(i) && keywordArguments.ContainsKey(parameterName))
            {
                throw new Exception("Parameter " + parameterName + " is addressed by a positional and keyword argument");
            } else if (positionalArguments.ContainsKey(i))
            {
                memory.SetVariable(new VariableInstruction(parameterName), positionalArguments[i]);
                positionalArguments.Remove(i);
            } else if (keywordArguments.ContainsKey(parameterName))
            {
                memory.SetVariable(new VariableInstruction(parameterName), keywordArguments[parameterName]);
                keywordArguments.Remove(parameterName);
            } else if (defaultValue is not null)
            {
                memory.SetVariable(new VariableInstruction(parameterName), defaultValue);
            }
            else
            {
                throw new Exception("Missing a required argument at arg " + (i + 1));
            }
        }
        
        if (positionalArguments.Count > 0)
        {
            throw new Exception("unknown positional arguments at " + positionalArguments.Keys.ToList());
        }

        if (keywordArguments.Count > 0)
        {
            throw new Exception("unknown keyword arguments at " + keywordArguments.Keys.ToList());
        }

        var returnValue = RunInstructions(memory);
            
        memory.RemoveScopes(classScopesAdded + 1);
        
        return returnValue ?? new NoneVariable();
    }

    private (string, Variable?) GetParameterNameAndDefaultValue(Memory memory, Instruction parameter, ObjectVariable? objectContext = null)
    {
        string parameterName;
        Variable? parameterDefaultValue = null;
        if (parameter is AssignmentInstruction assignmentInstruction)
        {
            parameterName = ((VariableInstruction)assignmentInstruction.Left).Name;
            parameterDefaultValue = assignmentInstruction.Right.Interpret(memory, objectContext);
        }
        else if (parameter is VariableInstruction variableInstruction)
        {
            parameterName = variableInstruction.Name;
        }
        else
        {
            throw new Exception("shouldn't get here");
        }
        
        return (parameterName, parameterDefaultValue);
    }

    public Variable RunFunction(Memory memory, List<Variable> arguments, ObjectVariable? objectVariable = null)
    {
        var positionalArguments = new Dictionary<int, Variable>();
        for (var i = 0; i < arguments.Count; i++)
        {
            positionalArguments[i] = arguments[i];
        }
        
        return RunFunctionWithArgumentsSorted(memory, positionalArguments, new Dictionary<string, Variable>(), objectVariable);
    }

    public Variable RunFunction(Memory memory, List<Instruction> arguments, ObjectVariable? objectVariable = null)
    {
        var positionalArguments = new Dictionary<int, Variable>();
        var keywordArguments = new Dictionary<string, Variable>();
        for (var i = 0; i < arguments.Count; i++)
        {
            var argument = arguments[i];
            if (argument is AssignmentInstruction assignmentInstruction)
            {
                var keywordName = ((VariableInstruction)assignmentInstruction.Left).Name;
                keywordArguments[keywordName] = assignmentInstruction.Right.Interpret(memory, objectVariable);
            }
            else
            {
                if (keywordArguments.Count > 0)
                {
                    throw new Exception("Positional arguments must go before keyword arguments, fix thsi later");
                }
                positionalArguments[i] = argument.Interpret(memory, objectVariable);
            }
            
        }
        
        return RunFunctionWithArgumentsSorted(memory, positionalArguments, keywordArguments, objectVariable);
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