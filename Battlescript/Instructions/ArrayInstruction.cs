namespace Battlescript;

public class ArrayInstruction : Instruction
{
    public string? Delimiter { get; private set; }
    public List<Instruction?> Values { get; private set; } = [];

    public ArrayInstruction(List<Token> tokens) : base(tokens)
    {
        InitializeDelimiter(tokens);
        InitializeValues(tokens);
    }

    protected void InitializeDelimiter(List<Token> tokens)
    {
        var commaIndex = InstructionUtilities.GetTokenIndex(tokens, [Consts.Comma]);
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [Consts.Colon]);

        // Prioritize grouping by commas over grouping by colons (ex: {3: 4, 5: 6})
        if (commaIndex != -1)
        {
            Delimiter = Consts.Comma;
        } else if (colonIndex != -1)
        {
            Delimiter = Consts.Colon;
        }
    }

    protected void InitializeValues(List<Token> tokens)
    {
        if (Delimiter is not null)
        {
            Values = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [Delimiter]);
        }
        else if (tokens.Count > 0)
        {
            Values = [InstructionFactory.Create(tokens)];
        }
    }

    public ArrayInstruction(
        List<Instruction?> values, 
        string? delimiter = null,
        Instruction? next = null,
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Values = values;
        Next = next;
        Delimiter = delimiter;
    }
}