namespace Battlescript;

public static class Truthiness
{
    public static bool IsTruthy(Memory memory, Variable variable)
    {
        switch (variable)
        {
            case ConstantVariable constantVariable:
                return constantVariable.Value == Consts.Constants.True;
            case NumericVariable numVariable:
                return numVariable.Value != 0;
            case StringVariable stringVariable:
                return stringVariable.Value.Length > 0;
            case ListVariable listVariable:
                return listVariable.Values.Count > 0;
            case ClassVariable:
                return true;
            case ObjectVariable:
                return IsObjectTruthy(memory, variable);
            case DictionaryVariable dictionaryVariable:
                return dictionaryVariable.IntValues.Count > 0 || dictionaryVariable.StringValues.Count > 0;
            case FunctionVariable:
                return true;
            default:
                throw new Exception("Won't get here");
        }
    }

    private static bool IsObjectTruthy(Memory memory, Variable variable)
    {
        var intObject = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "int", variable);
        if (intObject is not null)
        {
            var value = intObject.Values["__value"] as NumericVariable;
            return value.Value is bool ? value.Value : value.Value != 0;
        }
        
        var floatObject = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "float", variable);
        if (floatObject is not null)
        {
            var value = floatObject.Values["__value"] as NumericVariable;
            return Math.Abs(value.Value) > Consts.FloatingPointTolerance;
        }
        
        var boolObject = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "bool", variable);
        if (boolObject is not null)
        {
            var value = boolObject.Values["__value"] as NumericVariable;
            return value.Value == 1;
        }

        return true;
    }
}