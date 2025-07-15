namespace Battlescript;

public class ArrayInstruction : Instruction
{
    public string? Separator { get; private set; }
    public string? Delimiter { get; private set; }
    public List<Instruction?> Values { get; private set; } = [];

    public ArrayInstruction(List<Token> tokens)
    {
        if (tokens.First().Value == Consts.Period)
        {
            InitializeMember();
        }
        else if (Consts.OpeningSeparators.Contains(tokens.First().Value))
        {
            InitializeListWithSeparators();
        }
        else
        {
            InitializeListWithoutSeparators(tokens);
        }

        Line = tokens.First().Line;
        Column = tokens.First().Column;
        return;

        void InitializeMember()
        {
            Separator = Consts.SquareBrackets;
            Values = [new StringInstruction([tokens[1]])];
            ParseNext(tokens, 2);
        }

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
        Instruction? next = null)
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
        
            return new DictionaryVariable(intValues, stringValues);

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
                if (index is ArrayInstruction)
                {
                    var listVariable = index.Interpret(memory) as ListVariable;
                    indexVariable = listVariable.Values[0]!;
                }
                else
                {
                    indexVariable = index.Interpret(memory);
                }
        
                var intObject = BuiltInTypeHelper.IsVariableBuiltInClass(memory, "int", indexVariable);
                if (intObject is not null)
                {
                    return (BuiltInTypeHelper.GetIntValueFromVariable(memory, indexVariable), null);
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
                return functionVariable.RunFunction(memory, Values!, objectContext);
            }
            else if (instructionContext is ClassVariable classVariable)
            {
                var objectVariable = classVariable.CreateObject();
                RunConstructor(objectVariable);
                return objectVariable;
            }
            else
            {
                if (Values.Count == 1)
                {
                    return Values[0]!.Interpret(memory);
                }
                else
                {
                    throw new Exception("Parens must follow a function or class");
                }
            }

            void RunConstructor(ObjectVariable objectVariable)
            {
                var constructor = objectVariable.GetItem(memory, "__init__");
                if (constructor is FunctionVariable constructorVariable)
                {
                    constructorVariable.RunFunction(memory, Values, objectVariable);
                }
            }
        }
        
        Variable SquareBracketsInterpret()
        {
            if (instructionContext is not null)
            {
                return InterpretIndex();
            }
            else
            {
                return InterpretListCreation();
            }

            Variable InterpretIndex()
            {
                if (IsListMethod() && instructionContext is ListVariable listVariable)
                {
                    var stringInstruction = Values[0] as StringInstruction;
                    return listVariable.RunMethod(memory, stringInstruction!.Value, Next);
                }
                else
                {
                    return instructionContext.GetItem(memory, this, objectContext);
                }
            }

            bool IsListMethod()
            {
                return Values.First() is StringInstruction stringInstruction &&
                       Consts.ListMethods.Contains(stringInstruction.Value);
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

                return new ListVariable(values!);
            }
        }
    }
}