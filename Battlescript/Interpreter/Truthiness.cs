namespace Battlescript;

public static class Truthiness
{
    public static bool IsTruthy(CallStack callStack, Closure closure, Variable? variable, Instruction? inst = null)
    {
        switch (variable)
        {
            case null:
                return false;
            case ConstantVariable:
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
                var boolFunc = objectVariable.GetMember(callStack, closure, new MemberInstruction("__bool__"));
                if (boolFunc is FunctionVariable funcVariable)
                {
                    var result = funcVariable.RunFunction(callStack, closure, new ArgumentSet([]), inst);
                    return BtlTypes.Is(BtlTypes.Types.Bool, result) && BtlTypes.GetBoolValue(result);
                }
                return true;
            default:
                throw new Exception("Won't get here");
        }
    }
}