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
                    var powValue = isInverted
                        ? Math.Pow(otherNumeric.Value, _value)
                        : Math.Pow(_value, otherNumeric.Value);
                    return new NumericVariable(powValue);
                case "*":
                    return new NumericVariable(_value * otherNumeric.Value);
                case "/":
                    var divValue = isInverted
                        ? (double)otherNumeric.Value / (double)_value
                        : (double)_value / (double)otherNumeric.Value;
                    return new NumericVariable(divValue);
                case "//":
                    var floorDivValue = isInverted
                        ? Math.Floor((double)otherNumeric.Value / (double)_value)
                        : Math.Floor((double)_value / (double)otherNumeric.Value);
                    return new NumericVariable(floorDivValue);
                case "%":
                    var modValue = isInverted
                        ? otherNumeric.Value % _value
                        : _value % otherNumeric.Value;
                    return new NumericVariable(modValue);
                case "+":
                    return new NumericVariable(_value + otherNumeric.Value);
                case "-":
                    var subValue = isInverted
                        ? otherNumeric.Value - _value
                        : _value - otherNumeric.Value;
                    return new NumericVariable(subValue);
                case "==":
                    return new NumericVariable(Math.Abs(_value - otherNumeric.Value) < Consts.FloatingPointTolerance ? 1 : 0);
                case "!=":
                    return new NumericVariable(Math.Abs(_value - otherNumeric.Value) > Consts.FloatingPointTolerance ? 1 : 0);
                case ">":
                    var gValue = isInverted
                        ? otherNumeric.Value > _value
                        : _value > otherNumeric.Value;
                    return new NumericVariable(gValue ? 1 : 0);
                case ">=":
                    var geValue = isInverted
                        ? otherNumeric.Value >= _value
                        : _value >= otherNumeric.Value;
                    return new NumericVariable(geValue ? 1 : 0);
                case "<":
                    var lValue = isInverted
                        ? otherNumeric.Value < _value
                        : _value < otherNumeric.Value;
                    return new NumericVariable(lValue ? 1 : 0);
                case "<=":
                    var leValue = isInverted
                        ? otherNumeric.Value <= _value
                        : _value <= otherNumeric.Value;
                    return new NumericVariable(leValue ? 1 : 0);
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