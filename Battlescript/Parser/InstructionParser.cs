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
                case "def":
                    return HandleDef(tokens);
                case "return":
                    return HandleReturn(tokens);
                case "class":
                    return HandleClass(tokens);
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
            line: separatorToken.Line, 
            column: separatorToken.Column, 
            type: type,
            operation: separatorToken.Value,
            left: result.Left,
            right: result.Right
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
            line: tokens[0].Line,
            column: tokens[0].Column,
            type: type,
            valueList: results.Values,
            next: next
        );
    }
    
    private Instruction HandleMember(List<Token> tokens)
    {
        var indexValue = new Instruction(
            line: tokens[1].Line,
            column: tokens[1].Column,
            type: Consts.InstructionTypes.String,
            literalValue: tokens[1].Value
        );
        var next = CheckAndRunFollowingTokens(tokens, 2);
        
        // It seems like the easiest way to handle using the period for accessing members is to treat it exactly
        // like a square bracket (i.e. x.asdf = x["asdf"]).  This may change later once I know python better :P
        return new Instruction(
            line: tokens[0].Line,
            column: tokens[0].Column,
            type: Consts.InstructionTypes.SquareBrackets,
            valueList: new List<Instruction> { indexValue },
            next: next
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
            line: tokens[0].Line,
            column: tokens[0].Column,
            type: type,
            valueList: results.Values
        );
    }
    
    private Instruction HandleOperation(List<Token> tokens, int operatorIndex)
    {
        var operatorToken = tokens[operatorIndex];
        var result = RunLeftAndRightAroundIndex(tokens, operatorIndex);
        return new Instruction(
            line: operatorToken.Line, 
            column: operatorToken.Column, 
            type: Consts.InstructionTypes.Operation,
            operation: operatorToken.Value,
            left: result.Left,
            right: result.Right
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
            line: tokens[0].Line,
            column: tokens[0].Column,
            type: Consts.InstructionTypes.If,
            value: condition
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
            line: tokens[0].Line,
            column: tokens[0].Column,
            type: Consts.InstructionTypes.While,
            value: condition
        );
    }

    private Instruction HandleDef(List<Token> tokens)
    {
        // def keyword, function name, open parens, close parens, and colon
        if (tokens.Count < 5)
        {
            ThrowErrorForToken("Invalid function definition", tokens[0]);
        }
        
        if (tokens[^1].Value != ":")
        {
            ThrowErrorForToken("Function definition should end with colon", tokens[0]);
        }
        
        if (tokens[1].Type != Consts.TokenTypes.Identifier)
        {
            ThrowErrorForToken("Invalid function name", tokens[1]);
        }

        if (tokens[2].Type != Consts.TokenTypes.Separator || tokens[2].Value != "(")
        {
            ThrowErrorForToken("Expected ( for function definition", tokens[2]);
        }
        
        if (tokens[^2].Type != Consts.TokenTypes.Separator || tokens[^2].Value != ")")
        {
            ThrowErrorForToken("Expected ) for function definition", tokens[^2]);
        }

        var tokensInParens = tokens.GetRange(2, tokens.Count - 2);
        var results = ParseAndRunEntriesWithinSeparator(tokensInParens, [","]);
        return new Instruction(
            line: tokens[0].Line,
            column: tokens[0].Column,
            type: Consts.InstructionTypes.Function,
            name: tokens[1].Value,
            valueList: results.Values
        );
    }

    private Instruction HandleReturn(List<Token> tokens)
    {
        var value = Run(tokens.GetRange(1, tokens.Count - 1));
        return new Instruction(
            line: tokens[0].Line, 
            column: tokens[0].Column, 
            type: Consts.InstructionTypes.Return, 
            value: value);
    }
    
    private Instruction HandleClass(List<Token> tokens)
    {
        List<Instruction>? superClasses = null;
        if (tokens.Count > 3)
        {
            var tokensInParens = tokens.GetRange(2, tokens.Count - 3);
            superClasses = ParseAndRunEntriesWithinSeparator(tokensInParens, [","]).Values;
        }

        return new Instruction(
            line: tokens[0].Line, 
            column: tokens[0].Column, 
            type: Consts.InstructionTypes.Class, 
            name: tokens[1].Value,
            valueList: superClasses);
    }

    private Instruction HandleIdentifier(List<Token> tokens)
    {
        var next = CheckAndRunFollowingTokens(tokens, 1);
        
        return new Instruction(
            line: tokens[0].Line, 
            column: tokens[0].Column, 
            type: Consts.InstructionTypes.Variable, 
            name: tokens[0].Value, 
            next: next
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
        
        return new Instruction(line: tokens[0].Line, column: tokens[0].Column, type: instructionType, literalValue: value);
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
                    new Instruction(type: Consts.InstructionTypes.KeyValuePair, operation: ":", left: kvp.Left, right: kvp.Right));
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