namespace Battlescript;

public static class BsTypes
{
    public static bool Is(Memory memory, string builtInType, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences[builtInType];
        return variable is ObjectVariable objectVariable && objectVariable.Class.Name == builtInClass.Name;
    }
    
    public static Variable Create(Memory memory, string builtInType, dynamic value)
    {
        var builtInClass = memory.BuiltInReferences[builtInType];
        var objectVariable = builtInClass.CreateObject();

        if (value is int || value is double)
        {
            objectVariable.Values["__value"] = new NumericVariable(value);
            return objectVariable;
        } else if (value is bool)
        {
            objectVariable.Values["__value"] = new NumericVariable(value ? 1 : 0);
            return objectVariable;
        } else if (value is List<Variable>)
        {
            objectVariable.Values["__value"] = new SequenceVariable(value);
            return objectVariable;
        }
        
        objectVariable.Values["__value"] = value;
        return objectVariable;
    }
    
    public static int GetIntValue(Memory memory, Variable variable)
    {
        if (Is(memory, "int", variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }
    
    public static double GetFloatValue(Memory memory, Variable variable)
    {
        if (Is(memory, "float", variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not a float");
        }
    }
    
    public static bool GetBoolValue(Memory memory, Variable variable)
    {
        if (Is(memory, "bool", variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value != 0;
        }
        else
        {
            throw new Exception("Variable is not a bool");
        }
    }
    
    public static SequenceVariable GetListValue(Memory memory, Variable variable)
    {
        if (Is(memory, "list", variable) && variable is ObjectVariable objectVariable)
        {
            var valueVariable = objectVariable.Values["__value"] as SequenceVariable;
            return valueVariable!;
        }
        else
        {
            throw new Exception("Variable is not a list");
        }
    }
}