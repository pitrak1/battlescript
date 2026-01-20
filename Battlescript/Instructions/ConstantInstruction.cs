namespace Battlescript;

public class ConstantInstruction : Instruction, IEquatable<ConstantInstruction>
{
    public string Value { get; set; }

    public ConstantInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[0].Value;
    }

    public ConstantInstruction(string value, int? line = null, string? expression = null) : base(line, expression)
    {
        Value = value;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        switch (Value)
        {
            case "True":
                return BtlTypes.True;
            case "False":
                return BtlTypes.False;
            default:
                return BtlTypes.None;
        }
    }

    #region Equality

    public override bool Equals(object? obj) => obj is ConstantInstruction inst && Equals(inst);

    public bool Equals(ConstantInstruction? other) =>
        other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ConstantInstruction? left, ConstantInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ConstantInstruction? left, ConstantInstruction? right) => !(left == right);

    #endregion
}