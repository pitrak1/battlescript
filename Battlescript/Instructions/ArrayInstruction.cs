namespace Battlescript;

public class ArrayInstruction : Instruction
{
    public enum BracketTypes { CurlyBraces, SquareBrackets, Parentheses, None }
    public BracketTypes Bracket { get; private set; } = BracketTypes.None;
    private Dictionary<string, BracketTypes> StringToBracketTypesMapping = new Dictionary<string, BracketTypes>()
    {
        { "{", BracketTypes.CurlyBraces },
        { "[", BracketTypes.SquareBrackets },
        { "(", BracketTypes.Parentheses }
    };
    
    
    public enum DelimiterTypes { Comma, Colon, None }
    public DelimiterTypes Delimiter { get; private set; } = DelimiterTypes.None;
    private Dictionary<DelimiterTypes, string> DelimiterTypesToStringMapping = new Dictionary<DelimiterTypes, string>()
    {
        { DelimiterTypes.Comma, "," },
        { DelimiterTypes.Colon, ":"}
    };
    
    public List<Instruction?> Values { get; private set; } = [];

    public ArrayInstruction(List<Token> tokens) : base(tokens)
    {
        if (Consts.OpeningBrackets.Contains(tokens[0].Value))
        {
            Bracket = StringToBracketTypesMapping[tokens[0].Value];
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
            Values = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [DelimiterTypesToStringMapping[Delimiter]]);
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
        var stringValues = new Dictionary<string, Variable>();
        var intValues = new Dictionary<int, Variable>();
    
        // Dictionaries will be an ArrayInstruction with colon delimiters and two values within an ArrayInstruction
        // of comma delimiters if there are multiple entries. Dictionaries will be an ArrayInstruction of colon
        // delimiters with two values if there is only one entry.
        if (Delimiter == DelimiterTypes.Comma)
        {
            foreach (var dictValue in Values)
            {
                InterpretAndAddKvp(callStack, closure, stringValues, intValues, dictValue!);
            }
        } 
        else if (Delimiter == DelimiterTypes.Colon)
        {
            InterpretAndAddKvp(callStack, closure, stringValues, intValues, this);
        } else if (Values.Count != 0)
        {
            throw new Exception("Badly formed dictionary");
        }
    
        return BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(intValues, stringValues));
    }
    
    private void InterpretAndAddKvp(CallStack callStack, Closure closure, Dictionary<string, Variable> stringValues, Dictionary<int, Variable> intValues, Instruction? instruction)
    {
        var kvp = IsValidKvp(instruction);
        var value = kvp.Values[1]!.Interpret(callStack, closure); 
        var indexValue = GetIndexValue(callStack, closure, kvp.Values[0]!);
        if (indexValue.IntValue is not null)
        {
            intValues.Add(indexValue.IntValue.Value, value);
        }
        else
        {
            stringValues.Add(indexValue.StringValue!, value);
        }
    }

    private ArrayInstruction IsValidKvp(Instruction? instruction)
    {
        if (instruction is ArrayInstruction { Delimiter: DelimiterTypes.Colon, Values: [not null, not null] } kvpInstruction)
        {
            return kvpInstruction;
        }
        else
        {
            throw new Exception("Badly formed dictionary");
        }
    }
        
    private (int? IntValue, string? StringValue) GetIndexValue(CallStack callStack, Closure closure, Instruction index)
    {
        Variable indexVariable;
        if (index is ArrayInstruction indexInst)
        {
            var listVariable = indexInst.Values.Select(x => x.Interpret(callStack, closure)).ToList();
            indexVariable = listVariable[0]!;
        }
        else
        {
            indexVariable = index.Interpret(callStack, closure);
        }
    
        if (BsTypes.Is(BsTypes.Types.Int, indexVariable))
        {
            return (BsTypes.GetIntValue(indexVariable), null);
        } else if (BsTypes.Is(BsTypes.Types.String, indexVariable))
        {
            return (null, BsTypes.GetStringValue(indexVariable));
        }
        else
        {
            throw new Exception("Invlaid dictionary index, must be int or string");
        }
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
            var constructor = objectVariable.GetMember(callStack, closure, new MemberInstruction("__init__"));
            if (constructor is FunctionVariable constructorVariable)
            {
                constructorVariable.RunFunction(callStack, closure, new ArgumentSet(callStack, closure, Values!), this);
            }
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
        var values = new List<Variable>();
        
        foreach (var instructionValue in Values)
        {
            if (instructionValue is not null)
            {
                values.Add(instructionValue.Interpret(callStack, closure));
            }
            else
            {
                throw new Exception("Poorly formed list initialization");
            }
            
        }

        return BsTypes.Create(BsTypes.Types.List, values);
    }
}