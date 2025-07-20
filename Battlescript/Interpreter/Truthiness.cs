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
        if (BsTypes.Is(memory, "int", variable))
        {
            var value = (variable as ObjectVariable).Values["__value"] as NumericVariable;
            return value.Value is bool ? value.Value : value.Value != 0;
        }
        
        if (BsTypes.Is(memory, "float", variable))
        {
            var value = (variable as ObjectVariable).Values["__value"] as NumericVariable;
            return Math.Abs(value.Value) > Consts.FloatingPointTolerance;
        }
        
        if (BsTypes.Is(memory, "bool", variable))
        {
            var value = (variable as ObjectVariable).Values["__value"] as NumericVariable;
            return value.Value == 1;
        }
        
        if (BsTypes.Is(memory, "list", variable))
        {
            var value = (variable as ObjectVariable).Values["__value"] as SequenceVariable;
            return value.Values.Count > 0;
        }

        return true;
    }
}