namespace Battlescript;

public static class Postlexer
{
    public static void Run(List<Token> tokens)
    {
        JoinIsNotAndNotIn(tokens);
        CheckForParenthesesWithBuiltInFunctionCalls(tokens);
        CheckForMatchingSeparators(tokens);
        CheckForFormattedStrings(tokens);
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
                    BsTypes.Types.SyntaxError, 
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
            return i >= tokens.Count - 1 || tokens[i + 1] is not { Type: Consts.TokenTypes.Separator, Value: "(" };
        }
    }

    private static void CheckForMatchingSeparators(List<Token> tokens)
    {
        List<string> separatorStack = [];
    
        foreach (var token in tokens)
        {
            if (IsOpeningSeparator(token))
            {
                separatorStack.Add(token.Value);
            } else if (IsClosingSeparator(token))
            {
                if (MatchesPreviousOpeningSeparator(token))
                {
                    separatorStack.RemoveAt(separatorStack.Count - 1);
                }
                else
                {
                    var message = "closing parenthesis '" + token.Value + "' does not match opening parenthesis '" + separatorStack[^1] + "'";
                    throw new InternalRaiseException(BsTypes.Types.SyntaxError, message);
                }
            }
        }

        if (separatorStack.Count != 0)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "unexpected EOF while parsing");
        }

        return;
        
        bool IsOpeningSeparator(Token t)
        {
            return Consts.OpeningSeparators.Contains(t.Value);
        }
            
        bool IsClosingSeparator(Token t)
        {
            return Consts.ClosingSeparators.Contains(t.Value);
        }

        bool MatchesPreviousOpeningSeparator(Token t)
        {
            return Consts.MatchingSeparatorsMap[t.Value] == separatorStack[^1];
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