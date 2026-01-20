namespace Battlescript;

public class StringInstruction : Instruction, IEquatable<StringInstruction>
{
    public string Value { get; set; }
    public bool IsFormatted { get; set; }

    public StringInstruction(List<Token> tokens) : base(tokens)
    {
        Value = tokens[0].Value;
        IsFormatted = tokens[0].Type == Consts.TokenTypes.FormattedString;
    }

    public StringInstruction(
        string value,
        bool isFormatted = false,
        int? line = null,
        string? expression = null) : base(line, expression)
    {
        Value = value;
        IsFormatted = isFormatted;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var value = Value;
        if (IsFormatted)
        {
            value = StringUtilities.GetFormattedStringValue(callStack, closure, value);
        }
        return BtlTypes.Create(BtlTypes.Types.String, value);
    }

    #region Equality

    public override bool Equals(object? obj) => obj is StringInstruction inst && Equals(inst);

    public bool Equals(StringInstruction? other) =>
        other is not null && Value == other.Value && IsFormatted == other.IsFormatted;

    public override int GetHashCode() => HashCode.Combine(Value, IsFormatted);

    public static bool operator ==(StringInstruction? left, StringInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(StringInstruction? left, StringInstruction? right) => !(left == right);

    #endregion
}