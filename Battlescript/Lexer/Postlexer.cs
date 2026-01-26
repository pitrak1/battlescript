namespace Battlescript;

public static class Postlexer
{
    public static void Run(List<Token> tokens)
    {
        CollapseConsecutiveNewlines(tokens);
        JoinIsNotAndNotIn(tokens);
        CheckForParenthesesWithBuiltInFunctionCalls(tokens);
        CheckForMatchingBrackets(tokens);
        CheckForFormattedStrings(tokens);
    }

    private static void CollapseConsecutiveNewlines(List<Token> tokens)
    {
        // Collapse consecutive Newline tokens into a single one, keeping the last.
        // This handles blank lines - we only care about the indent of the next
        // line with actual content, not intermediate blank lines.
        var i = 0;
        while (i < tokens.Count - 1)
        {
            if (tokens[i].Type == Consts.TokenTypes.Newline &&
                tokens[i + 1].Type == Consts.TokenTypes.Newline)
            {
                tokens.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    private static void JoinIsNotAndNotIn(List<Token> tokens)
    {
        var i = 0;
        while (i < tokens.Count)
        {
            if (IsCurrentTokenNotKeyword() && IsPreviousTokenIsKeyword())
            {
                tokens[i - 1].Value = "is not";
                tokens.RemoveAt(i);
            }
            else if (IsCurrentTokenNotKeyword() && IsNextTokenInKeyword())
            {
                tokens[i + 1].Value = "not in";
                tokens.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        return;

        bool IsCurrentTokenNotKeyword()
        {
            return tokens[i] is { Type: Consts.TokenTypes.Operator, Value: "not" };
        }

        bool IsPreviousTokenIsKeyword()
        {
            return i > 0 && tokens[i - 1] is { Type: Consts.TokenTypes.Operator, Value: "is" };
        }

        bool IsNextTokenInKeyword()
        {
            return i < tokens.Count - 1 && tokens[i + 1] is { Type: Consts.TokenTypes.Operator, Value: "in" };
        }
    }

    private static void CheckForParenthesesWithBuiltInFunctionCalls(List<Token> tokens)
    {
        var i = 0;
        while (i < tokens.Count)
        {
            if (IsBuiltInToken() && IsNextTokenNotCloseParens())
            {
                throw new InternalRaiseException(
                    BtlTypes.Types.SyntaxError, 
                    $"Missing parentheses in call to '{tokens[i].Value}'");
            }
            i++;
        }

        bool IsBuiltInToken()
        {
            return tokens[i] is { Type: Consts.TokenTypes.BuiltIn };
        }
    
        bool IsNextTokenNotCloseParens()
        {
            return i >= tokens.Count - 1 || tokens[i + 1] is not { Type: Consts.TokenTypes.Bracket, Value: "(" };
        }
    }

    private static void CheckForMatchingBrackets(List<Token> tokens)
    {
        List<string> bracketStack = [];
    
        foreach (var token in tokens)
        {
            if (Consts.OpeningBrackets.Contains(token.Value))
            {
                bracketStack.Add(token.Value);
            } else if (Consts.ClosingBrackets.Contains(token.Value))
            {
                if (bracketStack.Count == 0)
                {
                    var message = "closing parenthesis '" + token.Value + "' has no matching opening parenthesis";
                    throw new InternalRaiseException(BtlTypes.Types.SyntaxError, message);
                } else if (MatchesPreviousOpeningBracket(token))
                {
                    bracketStack.RemoveAt(bracketStack.Count - 1);
                }
                else
                {
                    var message = "closing parenthesis '" + token.Value + "' does not match opening parenthesis '" + bracketStack[^1] + "'";
                    throw new InternalRaiseException(BtlTypes.Types.SyntaxError, message);
                }
            }
        }

        if (bracketStack.Count != 0)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "unexpected EOF while parsing");
        }

        return;
        
        bool MatchesPreviousOpeningBracket(Token t)
        {
            return Consts.MatchingBracketsMap[t.Value] == bracketStack[^1];
        }
    }

    private static void CheckForFormattedStrings(List<Token> tokens)
    {
        var i = 0;
        while (i < tokens.Count)
        {
            if (IsCurrentTokenFormattedStringIdentifier() && IsNextTokenString())
            {
                tokens[i + 1].Type = Consts.TokenTypes.FormattedString;
                tokens.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        return;

        bool IsCurrentTokenFormattedStringIdentifier()
        {
            return tokens[i] is { Type: Consts.TokenTypes.Identifier, Value: "f" };
        }

        bool IsNextTokenString()
        {
            return i < tokens.Count - 1 && tokens[i + 1].Type == Consts.TokenTypes.String;
        }
    }
}