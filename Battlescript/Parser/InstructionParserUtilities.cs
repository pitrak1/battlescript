namespace Battlescript;

public static class InstructionParserUtilities
{
    public static int GetAssignmentIndex(List<Token> tokens)
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == Consts.TokenTypes.Assignment)
            {
                return i;
            }
        }

        return -1;
    }

    public static int GetOperatorIndex(List<Token> tokens)
    {
        // Because we want to prioritize operators (and also prioritize earlier operators over later operators
        // of the same priority), we need to loop backwards and look for lower priority operators first.  The way
        // that these instructions are evaluated will be that the more deeply nested expressions will be evaluated
        // first.  Doing it this way means the lowest operator will split the entire expression with the left
        // and right operators containing variables, numbers, or sub-expressions with higher priority.

        var lowestOperatorPriority = -1;
        var lowestOperatorIndex = -1;
        
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            if (tokens[i].Type == Consts.TokenTypes.Operator)
            {
                // Because we want to find lower priority operators, the Operators const is in mathematical
                // priority descending order, making the higher priority operators have a lower value and
                // the lower priority operators have a higher value
                var priority = Array.FindIndex(Consts.Operators, e => e == tokens[i].Value);

                if (priority != -1 && priority > lowestOperatorPriority)
                {
                    lowestOperatorPriority = priority;
                    lowestOperatorIndex = i;
                }
            }
        }
        
        return lowestOperatorIndex;
    }

    public static (int Count, List<List<Token>> Entries) ParseTokensUntilMatchingSeparator(List<Token> tokens, List<string> separatingCharacters)
    {
        if (tokens.Count == 0)
        {
            return (0, []);
        }
        
        // The separator stacks allows us to ignore nested separators while checking that different types of 
        // separators match throughout the expression.  The stack will keep a record of the separators we are
        // currently nested in
        List<string> separatorStack = [];
        List<List<Token>> entries = [];
        List<Token> currentTokenSet = [];
        var totalTokenCount = 0;
        
        foreach (var token in tokens)
        {
            totalTokenCount++;

            if (Consts.OpeningSeparators.Contains(token.Value))
            {
                separatorStack.Add(token.Value);
                // We don't want to add the opening separator of the whole expression to the first entry
                if (totalTokenCount > 1)
                {
                    currentTokenSet.Add(token);
                }
            }
            else if (Consts.ClosingSeparators.Contains(token.Value))
            {
                var matchingCurrentTokenSeparator = Consts.MatchingSeparatorsMap[token.Value];
                if (matchingCurrentTokenSeparator == separatorStack[^1])
                {
                    // If the separator matches the top of the stack, pop it off. If the stack is now empty, that means
                    // we just hit the closing separator of the entire expression, so we add the last entry and exit
                    separatorStack.RemoveAt(separatorStack.Count - 1);
                    if (separatorStack.Count == 0)
                    {
                        entries.Add(currentTokenSet);
                        break;
                    }
                    
                    
                    currentTokenSet.Add(token);
                }
                else
                {
                    throw new Exception("Unexpected closing separator");
                }
            }
            // If we find a separating character at the base nesting of the expression, we want to store the entry
            // and start a new one
            else if (separatorStack.Count == 1 && separatingCharacters.Contains(token.Value))
            {
                entries.Add(currentTokenSet);
                currentTokenSet = [];
            }
            else
            {
                currentTokenSet.Add(token);
            }
        }

        // With the above code, an empty set of separators will result in a single empty entry in our list.  We'd
        // prefer to just have an empty set of entries
        return entries is [{ Count: 0 }] ? (totalTokenCount, []) : (totalTokenCount, entries);
    }
}