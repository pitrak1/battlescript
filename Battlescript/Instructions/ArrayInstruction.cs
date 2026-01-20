namespace Battlescript;

public class ArrayInstruction : Instruction, IEquatable<ArrayInstruction>
{
    public enum BracketTypes { CurlyBraces, SquareBrackets, Parentheses, None }
    public BracketTypes Bracket { get; private set; } = BracketTypes.None;
    private readonly Dictionary<string, BracketTypes> _stringToBracketTypesMapping = new Dictionary<string, BracketTypes>()
    {
        { "{", BracketTypes.CurlyBraces },
        { "[", BracketTypes.SquareBrackets },
        { "(", BracketTypes.Parentheses }
    };
    
    
    public enum DelimiterTypes { Comma, Colon, None }
    public DelimiterTypes Delimiter { get; private set; } = DelimiterTypes.None;
    private readonly Dictionary<DelimiterTypes, string> _delimiterTypesToStringMapping = new Dictionary<DelimiterTypes, string>()
    {
        { DelimiterTypes.Comma, "," },
        { DelimiterTypes.Colon, ":"}
    };
    
    public List<Instruction?> Values { get; private set; } = [];

    public ArrayInstruction(List<Token> tokens) : base(tokens)
    {
        if (Consts.OpeningBrackets.Contains(tokens[0].Value))
        {
            Bracket = _stringToBracketTypesMapping[tokens[0].Value];
            var tokensWithinBrackets = InstructionUtilities.GetGroupedTokensAtStart(tokens);
            InitializeDelimiter(tokensWithinBrackets);
            InitializeValues(tokensWithinBrackets);
            ParseNext(tokens, tokensWithinBrackets.Count + 2);
        }
        else
        {
            InitializeDelimiter(tokens);
            InitializeValues(tokens);
        }
    }

    private void InitializeDelimiter(List<Token> tokens)
    {
        var commaIndex = InstructionUtilities.GetTokenIndex(tokens, [Consts.Comma]);
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [Consts.Colon]);

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
            var delimiterString = _delimiterTypesToStringMapping[Delimiter];
            Values = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [delimiterString]);
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
        switch (Bracket)
        {
            case BracketTypes.CurlyBraces:
                return InterpretCurlyBraces(callStack, closure, instructionContext);
            case BracketTypes.SquareBrackets:
                return InterpretSquareBrackets(callStack, closure, instructionContext);
            case BracketTypes.Parentheses:
                return InterpretParentheses(callStack, closure, instructionContext);
            default:
                return null;
        }
    }
    
    private Variable? InterpretCurlyBraces(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var values = new MappingVariable();
    
        // A comma delimiter here means we have multiple kvps
        if (Delimiter == DelimiterTypes.Comma)
        {
            Values.ForEach(value => InterpretAndAddKvp(callStack, closure, values, value!));
        } 
        else
        {
            InterpretAndAddKvp(callStack, closure, values, this);
        }
    
        return BtlTypes.Create(BtlTypes.Types.Dictionary, values);
    }
    
    private void InterpretAndAddKvp(CallStack callStack, Closure closure, MappingVariable values, Instruction? instruction)
    {
        if (Values.Count == 0) return;
        if (!IsValidKvp(instruction)) throw new Exception("Invalid dictionary kv pair");
        var kvp = instruction as ArrayInstruction;
        
        var value = kvp!.Values[1]!.Interpret(callStack, closure);
        var key = kvp.Values[0]!.Interpret(callStack, closure);
        if (BtlTypes.Is(BtlTypes.Types.Int, key!))
        {
            values.IntValues.Add(BtlTypes.GetIntValue(key!), value!);
        }
        else
        {
            values.StringValues.Add(BtlTypes.GetStringValue(key!), value!);
        }
    }

    private bool IsValidKvp(Instruction? instruction)
    {
        return instruction is ArrayInstruction { Delimiter: DelimiterTypes.Colon, Values: [not null, not null] };
    }
        
    private Variable? InterpretParentheses(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        if (instructionContext is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, Values!), this);
        }
        else if (instructionContext is ClassVariable classVariable)
        {
            var objectVariable = classVariable.CreateObject();
            objectVariable.RunConstructor(callStack, closure, new ArgumentSet(callStack, closure, Values!), this);
            return objectVariable;
        }
        else
        {
            throw new Exception("Parens must follow a function or class");
        }
    }
    
    private Variable? InterpretSquareBrackets(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        // If square brackets follow something, it's an index.  Otherwise, it's list creation
        if (instructionContext is not null)
        {
            return instructionContext.GetItem(callStack, closure, this, instructionContext as ObjectVariable);
        }
        else
        {
            return InterpretListCreation(callStack, closure);
        }
    }
    
    private Variable InterpretListCreation(CallStack callStack, Closure closure)
    {
        var interpretedValues = Values.Select(value =>
        {
            if (value is null) throw new Exception("Poorly formed list initialization");
            return value.Interpret(callStack, closure);
        });
        
        return BtlTypes.Create(BtlTypes.Types.List, interpretedValues.ToList());
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