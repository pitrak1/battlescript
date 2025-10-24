namespace Battlescript;

public class ArrayInstruction : Instruction
{
    public string? Separator { get; private set; }
    public string? Delimiter { get; private set; }
    public List<Instruction?> Values { get; private set; } = [];

    public ArrayInstruction(List<Token> tokens) : base(tokens)
    {
        if (Consts.OpeningSeparators.Contains(tokens.First().Value))
        {
            InitializeListWithSeparators(tokens);
        }
        else
        {
            InitializeListWithoutSeparators(tokens);
        }
    }
    
    private void InitializeListWithSeparators(List<Token> tokens)
    {
        Separator = tokens.First().Value;
        var closingSeparator = Consts.MatchingSeparatorsMap[Separator];
        var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, [closingSeparator]);
        var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
        InitializeListWithoutSeparators(tokensInSeparators);
        ParseNext(tokens, closingSeparatorIndex + 1);
    }
        
    private void InitializeListWithoutSeparators(List<Token> tokens)
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
        string? separator = null,
        string? delimiter = null,
        Instruction? next = null) : base([])
    {
        Values = values;
        Next = next;
        Separator = separator;
        Delimiter = delimiter;
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Separator)
        {
            case Consts.CurlyBraces:
                return CurlyBracesInterpret(memory);
            case Consts.Parentheses:
                return ParenthesesInterpret(memory, instructionContext, objectContext);
            case Consts.SquareBrackets:
                return SquareBracketsInterpret(memory, instructionContext, objectContext);
            default:
                throw new Exception($"Unrecognized Separator: {Separator}, improve this later");
        }
    }
    
    private Variable CurlyBracesInterpret(Memory memory)
    {
        var stringValues = new Dictionary<string, Variable>();
        var intValues = new Dictionary<int, Variable>();
    
        // Dictionaries will be an ArrayInstruction with colon delimiters and two values within an ArrayInstruction
        // of comma delimiters if there are multiple entries. Dictionaries will be an ArrayInstruction of colon
        // delimiters with two values if there is only one entry.
        if (Delimiter == Consts.Comma)
        {
            foreach (var dictValue in Values)
            {
                InterpretAndAddKvp(memory, stringValues, intValues, dictValue!);
            }
        } 
        else if (Delimiter == Consts.Colon)
        {
            InterpretAndAddKvp(memory, stringValues, intValues, this);
        } else if (Values.Count != 0)
        {
            throw new Exception("Badly formed dictionary");
        }
    
        return memory.Create(Memory.BsTypes.Dictionary, new MappingVariable(intValues, stringValues));
    }
    
    private void InterpretAndAddKvp(Memory memory, Dictionary<string, Variable> stringValues, Dictionary<int, Variable> intValues, Instruction? instruction)
    {
        var kvp = IsValidKvp(instruction);
        var value = kvp.Values[1]!.Interpret(memory); 
        var indexValue = GetIndexValue(memory, kvp.Values[0]!);
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
        if (instruction is ArrayInstruction { Delimiter: ":", Values: [not null, not null] } kvpInstruction)
        {
            return kvpInstruction;
        }
        else
        {
            throw new Exception("Badly formed dictionary");
        }
    }
        
    private (int? IntValue, string? StringValue) GetIndexValue(Memory memory, Instruction index)
    {
        Variable indexVariable;
        if (index is ArrayInstruction indexInst)
        {
            var listVariable = indexInst.Values.Select(x => x.Interpret(memory)).ToList();
            indexVariable = listVariable[0]!;
        }
        else
        {
            indexVariable = index.Interpret(memory);
        }
    
        if (memory.Is(Memory.BsTypes.Int, indexVariable))
        {
            return (memory.GetIntValue(indexVariable), null);
        } else if (memory.Is(Memory.BsTypes.String, indexVariable))
        {
            return (null, memory.GetStringValue(indexVariable));
        }
        else
        {
            throw new Exception("Invlaid dictionary index, must be int or string");
        }
    }
    
    private Variable ParenthesesInterpret(Memory memory, Variable? instructionContext = null, ObjectVariable? objectContext = null)
    {
        if (instructionContext is FunctionVariable functionVariable)
        {
            // var stackUpdates = memory.CurrentStack.AddFrame(this, null, functionVariable.Name);
            var returnValue = functionVariable.RunFunction(memory, new ArgumentSet(memory, Values!, objectContext), this);
            // memory.CurrentStack.PopFrame(stackUpdates);
            return returnValue;
        }
        else if (instructionContext is ClassVariable classVariable)
        {
            var objectVariable = classVariable.CreateObject();
            RunConstructor(memory, objectVariable);
            return objectVariable;
        }
        else
        {
            throw new Exception("Parens must follow a function or class");
        }
    }
    
    private void RunConstructor(Memory memory, ObjectVariable objectVariable)
    {
        var constructor = objectVariable.Class.GetMember(memory, new MemberInstruction("__init__"));
        if (constructor is FunctionVariable constructorVariable)
        {
            // var stackUpdates = memory.CurrentStack.AddFrame(this, null, "__init__");
            constructorVariable.RunFunction(memory, new ArgumentSet(memory, Values!, objectVariable), this);
            // memory.CurrentStack.PopFrame(stackUpdates);
        }
    }
    
    private Variable SquareBracketsInterpret(Memory memory, Variable? instructionContext = null, ObjectVariable? objectContext = null)
    {
        if (instructionContext is not null)
        {
            return instructionContext.GetItem(memory, this, objectContext);
        }
        else
        {
            return InterpretListCreation(memory);
        }
    }
    
    private Variable InterpretListCreation(Memory memory)
    {
        var values = new List<Variable>();
        
        foreach (var instructionValue in Values)
        {
            if (instructionValue is not null)
            {
                values.Add(instructionValue.Interpret(memory));
            }
            else
            {
                throw new Exception("Poorly formed list initialization");
            }
            
        }

        return memory.Create(Memory.BsTypes.List, values);
    }
}