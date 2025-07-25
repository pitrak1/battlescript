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
            InitializeListWithSeparators();
        }
        else
        {
            InitializeListWithoutSeparators(tokens);
        }
        return;

        void InitializeListWithSeparators()
        {
            Separator = tokens.First().Value;
            var closingSeparator = Consts.MatchingSeparatorsMap[Separator];
            var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, [closingSeparator]);
            var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
            InitializeListWithoutSeparators(tokensInSeparators);
            ParseNext(tokens, closingSeparatorIndex + 1);
        }
        
        void InitializeListWithoutSeparators(List<Token> tokenList)
        {
            var commaIndex = InstructionUtilities.GetTokenIndex(tokenList, [Consts.Comma]);
            var colonIndex = InstructionUtilities.GetTokenIndex(tokenList, [Consts.Colon]);

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
                Values = InstructionUtilities.ParseEntriesBetweenDelimiters(tokenList, [Delimiter]);
            }
            else if (tokenList.Count > 0)
            {
                Values = [InstructionFactory.Create(tokenList)];
            }
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
    
    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        switch (Separator)
        {
            case Consts.CurlyBraces:
                return CurlyBracesInterpret();
            case Consts.Parentheses:
                return ParenthesesInterpret();
            case Consts.SquareBrackets:
                return SquareBracketsInterpret();
            default:
                throw new Exception($"Unrecognized Separator: {Separator}, improve this later");
        }

        Variable CurlyBracesInterpret()
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
                    InterpretAndAddKvp(dictValue!);
                }
            } 
            else if (Delimiter == Consts.Colon)
            {
                InterpretAndAddKvp(this);
            } else if (Values.Count != 0)
            {
                throw new Exception("Badly formed dictionary");
            }
        
            return BsTypes.Create(memory, BsTypes.Types.Dictionary, new MappingVariable(intValues, stringValues));

            void InterpretAndAddKvp(Instruction? instruction)
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

            ArrayInstruction IsValidKvp(Instruction? instruction)
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
            
            (int? IntValue, string? StringValue) GetIndexValue(Memory memory, Instruction index)
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
        
                if (BsTypes.Is(memory, BsTypes.Types.Int, indexVariable))
                {
                    return (BsTypes.GetIntValue(memory, indexVariable), null);
                } else if (indexVariable is StringVariable stringVariable)
                {
                    return (null, stringVariable.Value);
                }
                else
                {
                    throw new Exception("Invlaid dictionary index, must be int or string");
                }
            }
        }
        
        Variable ParenthesesInterpret()
        {
            if (instructionContext is FunctionVariable functionVariable)
            {
                return functionVariable.RunFunction(memory, Values!, objectContext, this);
            }
            else if (instructionContext is ClassVariable classVariable)
            {
                var objectVariable = classVariable.CreateObject();
                RunConstructor(objectVariable);
                return objectVariable;
            }
            else
            {
                throw new Exception("Parens must follow a function or class");
            }

            void RunConstructor(ObjectVariable objectVariable)
            {
                var constructor = objectVariable.Class.GetMember(memory, new MemberInstruction("__init__"));
                if (constructor is FunctionVariable constructorVariable)
                {
                    constructorVariable.RunFunction(memory, Values, objectVariable, this);
                }
            }
        }
        
        Variable SquareBracketsInterpret()
        {
            if (instructionContext is not null)
            {
                return instructionContext.GetItem(memory, this, objectContext);
            }
            else
            {
                return InterpretListCreation();
            }

            Variable InterpretListCreation()
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

                return BsTypes.Create(memory, BsTypes.Types.List, values);
            }
        }
    }
}