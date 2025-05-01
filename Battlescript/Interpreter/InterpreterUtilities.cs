namespace Battlescript;

public static class InterpreterUtilities
{
    public static Variable ConductOperation(string operation, Variable left, Variable right)
    {
        dynamic? result;
        Consts.VariableTypes type;
        
        switch (operation)
        {
            case "**":
                result = Math.Pow(left.Value, right.Value);
                type = Consts.VariableTypes.Number;
                break;
            case "~":
                result = ~(int)right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "*":
                result = left.Value * right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "/":
                result = left.Value / right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "//":
                result = (int)Math.Floor(left.Value / right.Value);
                type = Consts.VariableTypes.Number;
                break;
            case "%":
                result = left.Value % right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "+":
                result = left.Value + right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "-":
                result = left.Value - right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "<<":
                result = (int)left.Value << (int)right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case ">>":
                result = (int)left.Value >> (int)right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "&":
                result = (int)left.Value & (int)right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "^":
                result = (int)left.Value ^ (int)right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "|":
                result = (int)left.Value | (int)right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "==":
                result = left.Value == right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "!=":
                result = left.Value != right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case ">":
                result = left.Value > right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case ">=":
                result = left.Value >= right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "<":
                result = left.Value < right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "<=":
                result = left.Value <= right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "not":
                result = !right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "and":
                result = left.Value && right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "or":
                result = left.Value || right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            default:
                throw new SystemException("Invalid operator");
        }
        
        return new Variable(type, result);
    }

    public static Variable ConductAssignment(string operation, Variable left, Variable right)
    {
        var result = right;
        
        switch (operation)
        {
            case "+=":
            result.Value = left.Value + right.Value;
            break;
            case "-=":
            result.Value = left.Value - right.Value;
            break;
            case "*=":
            result.Value = left.Value * right.Value;
            break;
            case "/=":
            result.Value = left.Value / right.Value;
            break;
            case "%=":
            result.Value = left.Value % right.Value;
            break;
            case "//=":
            result.Value = Math.Floor(left.Value / right.Value);
            break;
            case "**=":
            result.Value = Math.Pow(left.Value, right.Value);
            break;
            case "&=":
            result.Value = (int)left.Value & (int)right.Value;
            break;
            case "|=":
            result.Value = (int)left.Value | (int)right.Value;
            break;
            case "^=":
            result.Value = (int)left.Value ^ (int)right.Value;
            break;
            case ">>=":
            result.Value = (int)left.Value >> (int)right.Value;
            break;
            case "<<=":
            result.Value = (int)left.Value << (int)right.Value;
            break;
        }

        return result;
    }

    public static bool IsVariableTruthy(Variable variable)
    {
        return variable is { Type: Consts.VariableTypes.Boolean, Value: true };
    }
}