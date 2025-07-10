namespace Battlescript;

public static class BuiltInTypeHelper
{
    public static ObjectVariable? IsVariableBuiltInClass(Memory memory, string builtInType, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences[builtInType];
        if (variable is ObjectVariable objectVariable && objectVariable.Class.Equals(builtInClass))
        {
            return objectVariable;
        }

        return null;
    }

    public static bool IsVariableAnyBuiltInClass(Memory memory, Variable variable)
    {
        foreach (var builtInClass in memory.BuiltInReferences.Values)
        {
            if (variable is ObjectVariable objectVariable && objectVariable.Class.Equals(builtInClass))
            {
                return true;
            }
        }

        return false;
    }
    
    public static int GetIntValueFromVariable(Memory memory, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences["int"];
        if (variable is ObjectVariable objectVariable && objectVariable.Class.Equals(builtInClass))
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }
    
    public static int GetStringValueFromVariable(Memory memory, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences["string"];
        if (variable is ObjectVariable objectVariable && objectVariable.Class.Equals(builtInClass))
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }

    public static Variable CreateBuiltInTypeWithValue(Memory memory, string builtInType, dynamic value)
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
        }
        
        objectVariable.Values["__value"] = value;
        return objectVariable;
    }
}