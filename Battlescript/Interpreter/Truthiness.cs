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
            case MappingVariable mappingVariable:
                return mappingVariable.IntValues.Count > 0 || mappingVariable.StringValues.Count > 0;
            case FunctionVariable:
                return true;
            default:
                throw new Exception("Won't get here");
        }
    }

    private static bool IsObjectTruthy(Memory memory, Variable variable)
    {
        if (BsTypes.Is(memory, BsTypes.Types.Int, variable))
        {
            var value = BsTypes.GetIntValue(memory, variable);
            return value != 0;
        } 
        else if (BsTypes.Is(memory, BsTypes.Types.Float, variable))
        {
            var value = BsTypes.GetFloatValue(memory, variable);
            return Math.Abs(value) > Consts.FloatingPointTolerance;
        }
        else if (BsTypes.Is(memory, BsTypes.Types.Bool, variable))
        {
            var value = BsTypes.GetBoolValue(memory, variable);
            return value;
        }
        else if (BsTypes.Is(memory, BsTypes.Types.List, variable))
        {
            var value = BsTypes.GetListValue(memory, variable);
            return value.Values.Count > 0;
        }
        else if (BsTypes.Is(memory, BsTypes.Types.Dictionary, variable))
        {
            var value = BsTypes.GetDictValue(memory, variable);
            return value.IntValues.Count > 0 || value.StringValues.Count > 0;
        }
        else
        {
            return true;
        }
    }
}