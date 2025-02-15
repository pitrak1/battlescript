using System.Data;

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
        var colonIndex = InstructionParserUtilities.GetTokenIndex(tokens, [":"]);
        var operatorIndex = InstructionParserUtilities.GetOperatorIndex(tokens);
        
        if (assignmentIndex != -1)
        {
            return HandlePair(tokens, assignmentIndex);
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
                    return HandleDictionaryOrSetDefinition(tokens);
                default:
                    return ThrowErrorForToken("Unexpected token", tokens[0]);
            }
        }
        else if (tokens[0].Type == Consts.TokenTypes.Keyword)
        {
            switch (tokens[0].Value)
            {
                case "if":
                    return HandleIf(tokens);
                case "while":
                    return HandleWhile(tokens);
                default:
                    return ThrowErrorForToken("Unexpected token", tokens[0]);
            }
        }
        else if (colonIndex != -1)
        {
            return HandlePair(tokens, colonIndex);
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

    private Instruction HandlePair(List<Token> tokens, int separatorIndex)
    {
        var separatorToken = tokens[separatorIndex];
        var result = RunLeftAndRightAroundIndex(tokens, separatorIndex);
        var type = separatorToken.Value == ":" ? 
            Consts.InstructionTypes.KeyValuePair : 
            Consts.InstructionTypes.Assignment;
        return new Instruction(
            separatorToken.Line, 
            separatorToken.Column, 
            type,
            separatorToken.Value,
            null,
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

    private Instruction HandleDictionaryOrSetDefinition(List<Token> tokens)
    {
        var results = ParseAndRunEntriesWithinSeparator(tokens, [","]);
        var type = results.Values[0].Type == Consts.InstructionTypes.KeyValuePair ?
            Consts.InstructionTypes.DictionaryDefinition :
            Consts.InstructionTypes.SetDefinition;
        
        // There should be no characters following a dictionary or set definition
        CheckForNoFollowingTokens(tokens, results.Count);
        
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            type,
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
            null,
            result.Left,
            result.Right
        );
    }

    private Instruction HandleIf(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("If statement should end with colon", tokens[0]);
        }
    
        var condition = Run(tokens.GetRange(1, tokens.Count - 2));
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.If,
            condition
        );
    }
    
    private Instruction HandleWhile(List<Token> tokens)
    {
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("While statement should end with colon", tokens[0]);
        }
    
        var condition = Run(tokens.GetRange(1, tokens.Count - 2));
        return new Instruction(
            tokens[0].Line,
            tokens[0].Column,
            Consts.InstructionTypes.While,
            condition
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
            var colonIndex = entry.FindIndex(0, x => x.Value == ":");
            if (colonIndex != -1)
            {
                var kvp = RunLeftAndRightAroundIndex(entry, colonIndex);
                values.Add(
                    new Instruction(Consts.InstructionTypes.KeyValuePair, null, null, kvp.Left, kvp.Right));
            }
            else
            {
                values.Add(Run(entry));
            }
            
        }

        return (results.Count, values);
    }

    private (Instruction? Left, Instruction? Right) RunLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = index > 0 ? Run(tokens.GetRange(0, index)) : null;
        var right = index < tokens.Count - 1 ? 
            Run(tokens.GetRange(index + 1, tokens.Count - index - 1)) : 
            null;

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