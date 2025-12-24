namespace Battlescript;

public class StringInstruction : Instruction
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
        return BsTypes.Create(BsTypes.Types.String, value);
    }
}