namespace Battlescript;

public class InstructionParser
{
    public Instruction Run(List<Token> tokens)
    {
        var assignmentIndex = InstructionParserUtilities.GetTokenIndex(
            tokens, 
            null, 
            [Consts.TokenTypes.Assignment]
        );
        var operatorIndex = InstructionParserUtilities.GetOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
        {
            return HandleAssignment(tokens, assignmentIndex);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Separator)
        {
            switch (tokens[0].Value)
            {
                case "[": case "(":
                    return HandleStandardSeparator(tokens);
                case ".":
                    return HandleMember(tokens);
                case "{":
                    return HandleCurlyBraces(tokens);
                default:
                    return ThrowErrorForToken("Unexpected token", tokens[0]);
            }
        }
        else if (operatorIndex != -1)
        {
            return HandleOperation(tokens, operatorIndex);
        }
        else if (tokens[0].Type == Consts.TokenTypes.Identifier)
        {
            return HandleIdentifier(tokens);
        }
        else if (tokens[0].Type is Consts.TokenTypes.Number or Consts.TokenTypes.String or Consts.TokenTypes.Boolean)
        {
            return HandleLiteral(tokens, tokens[0].Type);
        }
        else
        {
            return ThrowErrorForToken("Unexpected token", tokens[0]);
        }
    }

    private Instruction HandleAssignment(List<Token> tokens, int assignmentIndex)
    {
        var assignmentToken = tokens[assignmentIndex];
        var result = RunLeftAndRightAroundIndex(tokens, assignmentIndex);
        return new Instruction(
            assignmentToken.Line, 
            assignmentToken.Column, 
            Consts.InstructionTypes.Assignment,
            assignmentToken.Value,
            result.Left,
            result.Right
        );
    }

    private Instruction HandleStandardSeparator(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
        var next = CheckAndRunFollowingTokens(tokens, results.Count);
        var type = tokens[0].Value == "(" ? 
            Consts.InstructionTypes.Parens : 
            Consts.InstructionTypes.SquareBrackets;
        
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            type,
            results.Values,
            next
        );
    }
    
    private Instruction HandleMember(List<Token> tokens)
    {
        var indexValue = new Instruction(
            tokens[1].Line,
            tokens[1].Column,
            Consts.InstructionTypes.String,
            tokens[1].Value
        );
        var next = CheckAndRunFollowingTokens(tokens, 2);
        
        // It seems like the easiest way to handle using the period for accessing members is to treat it exactly
        // like a square bracket (i.e. x.asdf = x["asdf"]).  This may change later once I know python better :P
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.SquareBrackets,
            new List<Instruction> { indexValue },
            next
        );
    }

    private Instruction HandleCurlyBraces(List<Token> tokens)
    {
        // A colon will indicate key value pairs within a dictionary definition
        if (InstructionParserUtilities.GetTokenIndex(tokens, [":"]) != -1)
        {
            return HandleDictionaryDefinition(tokens);
        }
        else
        {
            return HandleSetDefinition(tokens);
        }
    }

    private Instruction HandleDictionaryDefinition(List<Token> tokens)
    {
        var results = 
            InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, [","]);
            
        // There should be no characters following a dictionary definition
        CheckForNoFollowingTokens(tokens, results.Count);

        List<(Instruction? Key, Instruction? Value)> pairs = [];
        foreach (var entry in results.Entries)
        {
            var colonIndex = InstructionParserUtilities.GetTokenIndex(entry, [":"]);
            if (colonIndex == -1)
            {
                ThrowErrorForToken("Dictionary entry should have key value pair", entry[0]);
            }
                
            var result = RunLeftAndRightAroundIndex(entry, colonIndex);
            pairs.Add(result);
        }
            
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.DictionaryDefinition,
            pairs
        );
    }

    private Instruction HandleSetDefinition(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
            
        // There should be no characters following a set definition
        CheckForNoFollowingTokens(tokens, results.Count);
            
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.SetDefinition,
            results.Values
        );
    }

    private Instruction HandleOperation(List<Token> tokens, int operatorIndex)
    {
        var operatorToken = tokens[operatorIndex];
        var result = RunLeftAndRightAroundIndex(tokens, operatorIndex);
        return new Instruction(
            operatorToken.Line, 
            operatorToken.Column, 
            Consts.InstructionTypes.Operation,
            operatorToken.Value,
            result.Left,
            result.Right
        );
    }

    private Instruction HandleIdentifier(List<Token> tokens)
    {
        var next = CheckAndRunFollowingTokens(tokens, 1);
        
        return new Instruction(
            tokens[0].Line, 
            tokens[0].Column, 
            Consts.InstructionTypes.Variable, 
            tokens[0].Value, 
            next
        );
    }

    private Instruction HandleLiteral(List<Token> tokens, Consts.TokenTypes type)
    {
        CheckForNoFollowingTokens(tokens, 1);

        dynamic? value;
        Consts.InstructionTypes instructionType;
        switch (type)
        {
            case Consts.TokenTypes.Number:
                value = Convert.ToDouble(tokens[0].Value);
                instructionType = Consts.InstructionTypes.Number;
                break;
            case Consts.TokenTypes.String:
                value = tokens[0].Value;
                instructionType = Consts.InstructionTypes.String;
                break;
            default:
                value = tokens[0].Value == "True";
                instructionType = Consts.InstructionTypes.Boolean;
                break;
        }
        
        return new Instruction(tokens[0].Line, tokens[0].Column, instructionType, value);
    }

    private (int Count, List<Instruction> Values) ParseAndRunEntriesWithinSeparator(
        List<Token> tokens, 
        List<string> separators
    )
    {
        var results = InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, separators);
        
        List<Instruction> values = [];
        foreach (var entry in results.Entries)
        {
            values.Add(Run(entry));
        }

        return (results.Count, values);
    }

    private (Instruction? Left, Instruction? Right) RunLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = Run(tokens.GetRange(0, index));
        var right = Run(tokens.GetRange(index + 1, tokens.Count - index - 1));

        return (left, right);
    }

    private Instruction? CheckAndRunFollowingTokens(List<Token> tokens, int expectedCount)
    {
        return tokens.Count > expectedCount ? 
            Run(tokens.GetRange(expectedCount, tokens.Count - expectedCount)) : 
            null;
    }

    private void CheckForNoFollowingTokens(List<Token> tokens, int expectedCount)
    {
        if (tokens.Count > expectedCount)
        {
            ThrowErrorForToken("Unexpected token", tokens[expectedCount]);
        }
    }

    // I put a return type of Instruction here because the interpreter doesn't seem to recognize that this
    // function always throws
    private Instruction ThrowErrorForToken(string message, Token token)
    {
        throw new Exception(
            message + "\n" +
            "Line " + token.Line + ", Column " + token.Column + "\n" +
            "Value: " + token.Value
        );
    }
}