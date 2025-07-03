namespace Battlescript;

public class ArrayInstruction : Instruction, IEquatable<ArrayInstruction>
{
    public string? Separator { get; set; }
    public string? Delimiter { get; set; }
    public Instruction? Next { get; set; }
    public List<Instruction?> Values { get; set; } = [];

    public ArrayInstruction(List<Token> tokens)
    {
        // Must be an index
        if (tokens[0].Value == ".")
        {
            Values = [new StringInstruction(new List<Token>() { tokens[1] })];
            Separator = "[";
            
            if (tokens.Count > 2)
            {
                Next = InstructionFactory.Create(tokens.GetRange(2, tokens.Count - 2));
            }
        }
        else if (Consts.OpeningSeparators.Contains(tokens[0].Value))
        {
            Separator = tokens[0].Value;
            var closingSeparator = Consts.MatchingSeparatorsMap[tokens[0].Value];
            var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, [closingSeparator]);

            var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
            PopulateFromTokens(tokensInSeparators);
            
            if (tokens.Count > closingSeparatorIndex + 1)
            {
                Next = InstructionFactory.Create(
                    tokens.GetRange(
                        closingSeparatorIndex + 1, 
                        tokens.Count - closingSeparatorIndex - 1));
            }
        }
        else
        {
            PopulateFromTokens(tokens);
        }

        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }

    private void PopulateFromTokens(List<Token> tokens)
    {
        var commaIndex = InstructionUtilities.GetTokenIndex(tokens, [","]);
        var colonIndex = InstructionUtilities.GetTokenIndex(tokens, [":"]);

        // Prioritize grouping by commas over grouping by colons (ex: {3: 4, 5: 6})
        if (commaIndex != -1)
        {
            Delimiter = ",";
        } else if (colonIndex != -1)
        {
            Delimiter = ":";
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
        Instruction? next = null, 
        string? separator = null,
        string? delimiter = null)
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
            case "{":
                return CurlyBracesInterpret(memory, instructionContext, objectContext, lexicalContext);
            case "(":
                return ParenthesesInterpret(memory, instructionContext, objectContext, lexicalContext);
            case "[":
                return SquareBracketsInterpret(memory, instructionContext, objectContext, lexicalContext);
            default:
                throw new Exception($"Unrecognized Separator: {Separator}");
        }
    }

    private Variable CurlyBracesInterpret(
        Memory memory,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var variableValue = new Dictionary<Variable, Variable>();
        
        if (Delimiter == ",")
        {
            // More than 1 dictionary value
            foreach (var dictValue in Values)
            {
                if (dictValue is ArrayInstruction { Delimiter: ":", Values.Count: 2 } kvpArray)
                {
                    var key = kvpArray.Values[0].Interpret(memory);
                    var value = kvpArray.Values[1].Interpret(memory);
            
                    variableValue.Add(key, value);
                }
                else
                {
                    throw new Exception("Badly formed ditionary");
                }
            }
        } else if (Values.Count == 2 && Delimiter == ":")
        {
            // Just 1 dicitonary value
            var key = Values[0].Interpret(memory);
            var value = Values[1].Interpret(memory);
            
            variableValue.Add(key, value);
        }
        
        return new DictionaryVariable(variableValue);
    }
    
    private Variable ParenthesesInterpret(
        Memory memory,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (instructionContext is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(memory, Values, objectContext);
        }
        else
        {
            if (instructionContext is ClassVariable classVariable)
            {
                var objectVariable = classVariable.CreateObject();
                var constructor = objectVariable.GetItem(memory, "__init__");
                if (constructor is FunctionVariable constructorVariable)
                {
                    List<Variable> arguments = [];
                    foreach (var argument in Values)
                    {
                        arguments.Add(argument.Interpret(memory, objectVariable, objectContext));
                    }
                    
                    constructorVariable.RunFunction(memory, arguments, objectVariable);
                }
                return objectVariable;
            }
            else
            {
                throw new Exception("Can only create an object of a class");
            }
        }
    }
    
    private Variable SquareBracketsInterpret(
        Memory memory,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        // Dealing with an index
        if (instructionContext is not null)
        {
            if (Values[0] is StringInstruction stringInstruction &&
                Consts.ListMethods.Contains(stringInstruction.Value) &&
                instructionContext is ListVariable listVariable)
            {
                return listVariable.RunMethod(memory, stringInstruction.Value, Next);
            }
            else
            {
                return instructionContext.GetItem(memory, this, objectContext);
            }
        }
        // Dealing with list creation
        else
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

            return new ListVariable(values);
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