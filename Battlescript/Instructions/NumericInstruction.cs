namespace Battlescript;

public class NumericInstruction : Instruction, IEquatable<NumericInstruction>
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

    public NumericInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens.Count > 1)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
        }

        if (tokens[0].Value.Contains('.'))
        {
            _value = double.Parse(tokens[0].Value);
        }
        else
        {
            _value = int.Parse(tokens[0].Value);
        }
    }

    public NumericInstruction(dynamic value, int? line = null, string? expression = null) : base(line, expression)
    {
        _value = value;
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        if (Value is double)
        {
            return BtlTypes.Create(BtlTypes.Types.Float, Value);
        }
        else
        {
            return BtlTypes.Create(BtlTypes.Types.Int, Value);
        }
    }
    
    #region Equality

    public override bool Equals(object? obj) => obj is NumericInstruction inst && Equals(inst);

    public bool Equals(NumericInstruction? other) =>
        other is not null && Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(NumericInstruction? left, NumericInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(NumericInstruction? left, NumericInstruction? right) => !(left == right);

    #endregion
}