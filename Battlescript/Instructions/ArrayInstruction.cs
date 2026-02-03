namespace Battlescript;

public class ArrayInstruction : Instruction, IEquatable<ArrayInstruction>
{
    public enum BracketTypes { CurlyBraces, SquareBrackets, Parentheses, None }
    public BracketTypes Bracket { get; private set; } = BracketTypes.None;

    private static readonly Dictionary<string, BracketTypes> BracketTypesMapping = new()
    {
        { "{", BracketTypes.CurlyBraces },
        { "[", BracketTypes.SquareBrackets },
        { "(", BracketTypes.Parentheses }
    };

    private static readonly Dictionary<BracketTypes, IBracketInterpreter> Interpreters = new()
    {
        { BracketTypes.CurlyBraces, new CurlyBraceInterpreter() },
        { BracketTypes.SquareBrackets, new SquareBracketInterpreter() },
        { BracketTypes.Parentheses, new ParenthesesInterpreter() },
        { BracketTypes.None, new ParenthesesInterpreter() }
    };

    public enum DelimiterTypes { Comma, Colon, None }
    public DelimiterTypes Delimiter { get; private set; } = DelimiterTypes.None;

    private static readonly Dictionary<DelimiterTypes, string> DelimiterTypesToStringMapping = new()
    {
        { DelimiterTypes.Comma, "," },
        { DelimiterTypes.Colon, ":" }
    };

    public bool IsExplicitTuple { get; private set; } = false;

    public List<Instruction?> Values { get; private set; } = [];

    public ArrayInstruction(List<Token> tokens) : base(tokens)
    {
        InitializeDelimiter(tokens);
        
        if (Consts.OpeningBrackets.Contains(tokens[0].Value) && Delimiter == DelimiterTypes.None)
        {
            Bracket = BracketTypesMapping[tokens[0].Value];
            var tokensWithinBrackets = InstructionUtilities.GetGroupedTokensAtStart(tokens);
            InitializeDelimiter(tokensWithinBrackets);
            InitializeValues(tokensWithinBrackets);
            ParseNext(tokens, tokensWithinBrackets.Count + 2);
        }
        else
        {
            InitializeValues(tokens);
        }
    }

    private void InitializeDelimiter(List<Token> tokens)
    {
        var commaIndex = InstructionUtilities.GetTokenIndex(tokens, [","]);
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);

        // Prioritize grouping by commas over grouping by colons (ex: {3: 4, 5: 6})
        if (commaIndex != -1)
        {
            Delimiter = DelimiterTypes.Comma;
        } else if (colonIndex != -1)
        {
            Delimiter = DelimiterTypes.Colon;
        }
    }

    private void InitializeValues(List<Token> tokens)
    {
        if (Delimiter != DelimiterTypes.None)
        {
            var delimiterString = DelimiterTypesToStringMapping[Delimiter];
            Values = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [delimiterString]);

            // Force parse as tuple (ex: (1,) should be tuple of length 1)
            if (tokens[^1].Value == ",")
            {
                IsExplicitTuple = true;
                Values.RemoveAt(Values.Count - 1);
            }
        }
        else if (tokens.Count > 0)
        {
            Values = [InstructionFactory.Create(tokens)];
        }
    }

    public ArrayInstruction(
        List<Instruction?> values, 
        BracketTypes? bracket = null,
        DelimiterTypes? delimiter = null,
        Instruction? next = null,
        int? line = null, 
        string? expression = null) : base(line, expression)
    {
        Values = values;
        Bracket = bracket ?? BracketTypes.None;
        Delimiter = delimiter ?? DelimiterTypes.None;
        Next = next;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        return Interpreters[Bracket].Interpret(callStack, closure, this, instructionContext);
    }

    
    
    #region Equality

    public override bool Equals(object? obj) => obj is ArrayInstruction inst && Equals(inst);

    public bool Equals(ArrayInstruction? other) =>
        other is not null &&
        Values.SequenceEqual(other.Values) &&
        Bracket == other.Bracket &&
        Delimiter == other.Delimiter &&
        Equals(Next, other.Next);

    public override int GetHashCode() => HashCode.Combine(Values, Bracket, Delimiter, Next);

    public static bool operator ==(ArrayInstruction? left, ArrayInstruction? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ArrayInstruction? left, ArrayInstruction? right) => !(left == right);

    #endregion
}