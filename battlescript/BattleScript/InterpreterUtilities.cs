namespace BattleScript.Core;

public class InterpreterUtilities
{
    public static bool isTruthy(ScopeVariable var)
    {
        if (var.Value is null)
        {
            return false;
        }
        else if ((var.Value is int) && (var.Value == 0))
        {
            return false;
        }
        else if ((var.Value is string) && (var.Value == ""))
        {
            return false;
        }
        else if (var.Value is bool)
        {
            return var.Value;
        }
        else
        {
            return true;
        }
    }
}