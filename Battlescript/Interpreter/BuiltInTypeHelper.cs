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

    public static Variable CreateBuiltInTypeWithValue(Memory memory, string builtInType, dynamic value)
    {
        var builtInClass = memory.BuiltInReferences[builtInType];
        var objectVariable = builtInClass.CreateObject();

        if (value is int || value is double)
        {
            objectVariable.Values["__value"] = new NumericVariable(value);
            return objectVariable;
        }
        
        objectVariable.Values["__value"] = value;
        return objectVariable;
    }
}