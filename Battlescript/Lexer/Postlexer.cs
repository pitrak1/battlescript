namespace Battlescript;

public static class Postlexer
{
    public static void Run(List<Token> tokens)
    {
        JoinIsNotAndNotIn(tokens);
        CheckForParenthesesWithBuiltInFunctionCalls(tokens);
        CheckForMatchingSeparators(tokens);
    }

    private static void JoinIsNotAndNotIn(List<Token> tokens)
    {
        var i = 0;
        while (i < tokens.Count)
        {
            if (tokens[i].Type == Consts.TokenTypes.Operator && tokens[i].Value == "not")
            {
                if (i > 0 && 
                    tokens[i - 1].Type == Consts.TokenTypes.Operator && 
                    tokens[i - 1].Value == "is")
                {
                    tokens[i - 1].Value = "is not";
                    tokens.RemoveAt(i);
                    continue;
                } else if (i < tokens.Count - 1 &&
                           tokens[i + 1].Type == Consts.TokenTypes.Operator &&
                           tokens[i + 1].Value == "in")
                {
                    tokens[i + 1].Value = "not in";
                    tokens.RemoveAt(i);
                    continue;
                }
            }
            i++;
        }
    }

    private static void CheckForParenthesesWithBuiltInFunctionCalls(List<Token> tokens)
    {
        var i = 0;
        while (i < tokens.Count)
        {
            if (tokens[i].Type == Consts.TokenTypes.BuiltIn)
            {
                if (i >= tokens.Count - 1 || TokenIsNotParenthesis(tokens[i + 1]))
                {
                    throw new InternalRaiseException(Memory.BsTypes.SyntaxError, $"Missing parentheses in call to '{tokens[i].Value}'");
                }
            }
            i++;
        }
    
        bool TokenIsNotParenthesis(Token token)
        {
            return token.Type != Consts.TokenTypes.Separator || token.Value != "(";
        }
    }

    private static void CheckForMatchingSeparators(List<Token> tokens)
    {
        List<string> separatorStack = [];
    
        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];
            if (Consts.OpeningSeparators.Contains(currentToken.Value))
            {
                separatorStack.Add(currentToken.Value);
            } else if (Consts.ClosingSeparators.Contains(currentToken.Value))
            {
                if (Consts.MatchingSeparatorsMap[currentToken.Value] == separatorStack[^1])
                {
                    // If the separator matches the top of the stack, pop it off. If the stack is now empty, that means
                    // we just hit the closing separator of the entire expression, so we add the last entry and exit
                    separatorStack.RemoveAt(separatorStack.Count - 1);
                }
                else
                {
                    throw new ParserUnexpectedClosingSeparatorException(currentToken);
                }
            }
        }

        if (separatorStack.Count != 0)
        {
            throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "unexpected EOF while parsing");
        }
    }
}