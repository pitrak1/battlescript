namespace Battlescript;

public class NumericVariable : Variable
{
    private dynamic _value;
    public dynamic Value
    {
        get => _value;
        private set
        {
            if (value is not double && value is not int)
            {
                throw new Exception("Wrong type for numeric instruction");
            }
            else
            {
                _value = value;
            }
        }
    }

    public NumericVariable(dynamic value)
    {
        _value = value;
    }
    
    public override Variable Operate(Memory memory, string operation, Variable? other, bool isInverted = false)
    {
        if (other is NumericVariable otherNumeric)
        {
            switch (operation)
            {
                case "**":
                    return new NumericVariable(Math.Pow(_value, otherNumeric.Value));
                case "*":
                    return new NumericVariable(_value * otherNumeric.Value);
                case "/":
                    return new NumericVariable((double)_value / (double)otherNumeric.Value);
                case "//":
                    return new NumericVariable(Math.Floor((double)_value / (double)otherNumeric.Value));
                case "%":
                    return new NumericVariable(_value % otherNumeric.Value);
                case "+":
                    return new NumericVariable(_value + otherNumeric.Value);
                case "-":
                    return new NumericVariable(_value - otherNumeric.Value);
                case "==":
                    return new NumericVariable(Math.Abs(_value - otherNumeric.Value) < Consts.FloatingPointTolerance ? 1 : 0);
                case "!=":
                    return new NumericVariable(Math.Abs(_value - otherNumeric.Value) > Consts.FloatingPointTolerance ? 1 : 0);
                case ">":
                    return new NumericVariable(_value > otherNumeric.Value ? 1 : 0);
                case ">=":
                    return new NumericVariable(_value >= otherNumeric.Value ? 1 : 0);
                case "<":
                    return new NumericVariable(_value < otherNumeric.Value ? 1 : 0);
                case "<=":
                    return new NumericVariable(_value <= otherNumeric.Value ? 1 : 0);
                default:
                    throw new InterpreterInvalidOperationException(operation, this, other);
            }
        }
        else if (other is null)
        {
            switch (operation)
            {
                case "-":
                    return new NumericVariable(-_value);
                case "+":
                    return new NumericVariable(_value);
                default:
                    throw new InterpreterInvalidOperationException(operation, this, other);
            }
        }
        else
        {
            return other.Operate(memory, operation, this);
        }
    }
    
    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as NumericVariable);
    public bool Equals(NumericVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}