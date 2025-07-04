namespace Battlescript;

public class ArrayInstruction : Instruction, IEquatable<ArrayInstruction>
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
            InitializeNext(2);
        }

        void InitializeListWithSeparators()
        {
            Separator = tokens.First().Value;
            var closingSeparator = Consts.MatchingSeparatorsMap[Separator];
            var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, [closingSeparator]);
            var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
            InitializeListWithoutSeparators(tokensInSeparators);
            InitializeNext(closingSeparatorIndex + 1);
        }

        void InitializeNext(int expectedTokenCount)
        {
            if (tokens.Count > expectedTokenCount)
            {
                Next = InstructionFactory.Create(tokens.GetRange(expectedTokenCount, tokens.Count - expectedTokenCount));
            }
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
            var variableValue = new Dictionary<Variable, Variable>();
        
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
        
            return new DictionaryVariable(variableValue);

            void InterpretAndAddKvp(Instruction? instruction)
            {
                var kvp = IsValidKvp(instruction);
                var key = kvp.Values[0]!.Interpret(memory);
                var value = kvp.Values[1]!.Interpret(memory); 
                variableValue.Add(key, value);
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
                throw new Exception("Parens must follow a function or class");
            }

            void RunConstructor(ObjectVariable objectVariable)
            {
                var constructor = objectVariable.GetItem(memory, "__init__");
                if (constructor is FunctionVariable constructorVariable)
                {
                    // This needs to be changed when I rewrite function args.  This currently doesn't support keyword
                    // arguments like it should.
                    List<Variable> arguments = [];
                    foreach (var argument in Values)
                    {
                        arguments.Add(argument!.Interpret(memory, objectVariable, objectContext));
                    }
                
                    constructorVariable.RunFunction(memory, arguments, objectVariable);
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

    // All the code below is to override equality
    public override bool Equals(object obj) => Equals(obj as ArrayInstruction);
    public bool Equals(ArrayInstruction? instruction)
    {
        if (instruction is null) return false;
        if (ReferenceEquals(this, instruction)) return true;
        if (GetType() != instruction.GetType()) return false;
        
        if (Separator != instruction.Separator || Delimiter != instruction.Delimiter) return false;
        if (Next is not null && !Next.Equals(instruction.Next)) return false;
        if (!Values.SequenceEqual(instruction.Values)) return false;
        
        return base.Equals(instruction);
    }
    
    public override int GetHashCode() => HashCode.Combine(Separator, Delimiter, Values, Next);
}