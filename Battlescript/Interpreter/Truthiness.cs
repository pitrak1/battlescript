namespace Battlescript;

public static class Truthiness
{
    public static bool IsTruthy(Memory memory, Variable variable)
    {
        switch (variable)
        {
            case ConstantVariable constantVariable:
                return false;
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
            var value = BsTypes.GetIntValue(memory, variable);
            return value != 0;
        } 
        else if (BsTypes.Is(memory, "float", variable))
        {
            var value = BsTypes.GetFloatValue(memory, variable);
            return Math.Abs(value) > Consts.FloatingPointTolerance;
        }
        else if (BsTypes.Is(memory, "bool", variable))
        {
            var value = BsTypes.GetBoolValue(memory, variable);
            return value;
        }
        else if (BsTypes.Is(memory, "list", variable))
        {
            var value = BsTypes.GetListValue(memory, variable);
            return value.Values.Count > 0;
        }
        else
        {
            return true;
        }
    }
}