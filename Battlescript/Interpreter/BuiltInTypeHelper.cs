namespace Battlescript;

public static class BuiltInTypeHelper
{
    public static ObjectVariable? IsVariableBuiltInClass(Memory memory, string builtInType, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences[builtInType];
        if (variable is ObjectVariable objectVariable && objectVariable.Class.Name == builtInClass.Name)
        {
            return objectVariable;
        }

        return null;
    }
    
    public static int GetIntValueFromVariable(Memory memory, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences["int"];
        if (variable is ObjectVariable objectVariable && objectVariable.Class.Name.Equals(builtInClass.Name))
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not an int");
        }
    }
    
    public static double GetFloatValueFromVariable(Memory memory, Variable variable)
    {
        var builtInClass = memory.BuiltInReferences["float"];
        if (variable is ObjectVariable objectVariable && objectVariable.Class.Equals(builtInClass))
        {
            var valueVariable = objectVariable.Values["__value"];
            return ((NumericVariable)valueVariable).Value;
        }
        else
        {
            throw new Exception("Variable is not a float");
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