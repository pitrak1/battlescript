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
        if (memory.Is(Memory.BsTypes.Int, variable))
        {
            var value = memory.GetIntValue(variable);
            return value != 0;
        } 
        else if (memory.Is(Memory.BsTypes.Float, variable))
        {
            var value = memory.GetFloatValue(variable);
            return Math.Abs(value) > Consts.FloatingPointTolerance;
        }
        else if (memory.Is(Memory.BsTypes.Bool, variable))
        {
            var value = memory.GetBoolValue(variable);
            return value;
        }
        else if (memory.Is(Memory.BsTypes.List, variable))
        {
            var value = memory.GetListValue(variable);
            return value.Values.Count > 0;
        }
        else if (memory.Is(Memory.BsTypes.Dictionary, variable))
        {
            var value = memory.GetDictValue(variable);
            return value.IntValues.Count > 0 || value.StringValues.Count > 0;
        }
        else
        {
            return true;
        }
    }
}