namespace Battlescript;

public abstract class Instruction(int line = 0, int column = 0)
{
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;
    public List<Instruction> Instructions { get; set; } = [];

    public static Instruction Parse(List<Token> tokens)
    {
        var assignmentIndex = ParserUtilities.GetTokenIndex(
            tokens, 
            types: [Consts.TokenTypes.Assignment]
        );
        var colonIndex = ParserUtilities.GetTokenIndex(tokens, [":"]);
        var operatorIndex = ParserUtilities.GetOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
        {
            return new AssignmentInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Separator)
        {
            switch (tokens[0].Value)
            {
                case "[":
                    return new SquareBracketsInstruction(tokens);
                case "(":
                    return new ParensInstruction(tokens);
                case ".":
                    return new SquareBracketsInstruction(tokens, isMember: true);
                case "{":
                    return new DictionaryInstruction(tokens);
                default:
                    return ThrowErrorForToken("Unexpected token", tokens[0]);
            }
        }
        else if (tokens[0].Type == Consts.TokenTypes.Keyword)
        {
            switch (tokens[0].Value)
            {
                case "if":
                    return new IfInstruction(tokens);
                case "while":
                    return new WhileInstruction(tokens);
                case "def":
                    return new FunctionInstruction(tokens);
                case "return":
                    return new ReturnInstruction(tokens);
                case "class":
                    return new ClassInstruction(tokens);
                default:
                    return ThrowErrorForToken("Unexpected token", tokens[0]);
            }
        }
        else if (colonIndex != -1)
        {
            return new KeyValuePairInstruction(tokens);
        }
        else if (operatorIndex != -1)
        {
            return new OperationInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Identifier)
        {
            return new VariableInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Number)
        {
            return new NumberInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.String)
        {
            return new StringInstruction(tokens);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Boolean)
        {
            return new BooleanInstruction(tokens);
        }
        else
        {
            return ThrowErrorForToken("Unexpected token", tokens[0]);
        }
    }
    
    public static (Instruction? Left, Instruction? Right) RunLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = index > 0 ? Parse(tokens.GetRange(0, index)) : null;
        var right = index < tokens.Count - 1 ? 
            Parse(tokens.GetRange(index + 1, tokens.Count - index - 1)) : 
            null;

        return (left, right);
    }
    
    public static (int Count, List<Instruction> Values) ParseAndRunEntriesWithinSeparator(
        List<Token> tokens, 
        List<string> separators
    )
    {
        var results = ParserUtilities.ParseTokensUntilMatchingSeparator(tokens, separators);
        
        List<Instruction> values = [];
        foreach (var entry in results.Entries)
        {
            var colonIndex = entry.FindIndex(0, x => x.Value == ":");
            if (colonIndex != -1)
            {
                values.Add(new KeyValuePairInstruction(entry));
            }
            else
            {
                values.Add(Parse(entry));
            }
            
        }

        return (results.Count, values);
    }
    
    public static Instruction? CheckAndRunFollowingTokens(List<Token> tokens, int expectedCount)
    {
        return tokens.Count > expectedCount ? 
            Parse(tokens.GetRange(expectedCount, tokens.Count - expectedCount)) : 
            null;
    }

    public static void CheckForNoFollowingTokens(List<Token> tokens, int expectedCount)
    {
        if (tokens.Count > expectedCount)
        {
            ThrowErrorForToken("Unexpected token", tokens[expectedCount]);
        }
    }
    
    // I put a return type of Instruction here because the interpreter doesn't seem to recognize that this
    // function always throws
    public static Instruction ThrowErrorForToken(string message, Token token)
    {
        throw new Exception(
            message + "\n" +
            "Line " + token.Line + ", Column " + token.Column + "\n" +
            "Value: " + token.Value
        );
    }

    public abstract Variable Interpret(
        Memory memory,
        Variable? context = null,
        Variable? objectContext = null);

}