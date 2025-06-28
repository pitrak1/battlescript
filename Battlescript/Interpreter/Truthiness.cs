namespace Battlescript;

public static class Truthiness
{
    public static bool IsTruthy(Variable variable)
    {
        switch (variable)
        {
            case ConstantVariable constantVariable:
                return constantVariable.Value == Consts.Constants.True;
            case IntegerVariable integerVariable:
                return integerVariable.Value != 0;
            case FloatVariable floatVariable:
                return floatVariable.Value != 0;
            case StringVariable stringVariable:
                return stringVariable.Value.Length > 0;
            case ListVariable listVariable:
                return listVariable.Values.Count > 0;
            case ClassVariable:
                return true;
            case ObjectVariable:
                return true;
            case DictionaryVariable dictionaryVariable:
                return dictionaryVariable.Values.Count > 0;
            case FunctionVariable:
                return true;
            case ArrayVariable:
                return true;
            default:
                throw new Exception("Won't get here");
        }
    }
}