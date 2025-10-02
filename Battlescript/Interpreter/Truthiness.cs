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
            case MappingVariable mappingVariable:
                return mappingVariable.IntValues.Count > 0 || mappingVariable.StringValues.Count > 0;
            case FunctionVariable:
                return true;
            case ObjectVariable objectVariable:
                var boolFunc = objectVariable.GetMember(memory, new MemberInstruction("__bool__"));
                if (boolFunc is FunctionVariable funcVariable)
                {
                    var result = funcVariable.RunFunction(memory, new ArgumentSet([objectVariable]));
                    return memory.Is(Memory.BsTypes.Bool, result) && memory.GetBoolValue(result);
                }
                else
                {
                    return true;
                }
            default:
                throw new Exception("Won't get here");
        }
    }
}