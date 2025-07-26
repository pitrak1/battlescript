namespace Battlescript;

public class StringVariable : Variable, IEquatable<StringVariable>
{
    public string Value { get; set; }
    public StringVariable(string? value = null)
    {
        Value = value ?? "";
    }

    public override Variable Operate(Memory memory, string operation, Variable? other, bool isInverted = false)
    {
        if (other is StringVariable otherString)
        {
            switch (operation)
            {
                case "+":
                    return new StringVariable(Value + otherString.Value);
                case "==":
                    return memory.Create(Memory.BsTypes.Bool, Value == otherString.Value);
                default:
                    throw new InterpreterInvalidOperationException(operation, this, other);
            }
        }
        else if (other is null)
        {
            throw new InterpreterInvalidOperationException(operation, this, other);
        }
        else
        {
            return other.Operate(memory, operation, this);
        }
    }

    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as StringVariable);
    public bool Equals(StringVariable? variable)
    {
        if (variable is null) return false;
        if (ReferenceEquals(this, variable)) return true;
        if (GetType() != variable.GetType()) return false;
        
        return Value == variable.Value;
    }
    
    public override int GetHashCode() => HashCode.Combine(Value);
}