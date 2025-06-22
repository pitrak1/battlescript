using System.Diagnostics;

namespace Battlescript;

public static class ParserUtilities
{
    public static int GetTokenIndex(
        List<Token> tokens, 
        List<string>? values = null, 
        List<Consts.TokenTypes>? types = null
    )
    {
        List<string> separatorStack = [];
        
        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];
            if (IsTokenInValuesOrTypes(values, types, currentToken) && separatorStack.Count == 0)
            {
                return i;
            }
            else if (Consts.OpeningSeparators.Contains(currentToken.Value))
            {
                separatorStack.Add(currentToken.Value);
            } else if (Consts.ClosingSeparators.Contains(currentToken.Value))
            {
                if (DoesTokenMatchSeparator(currentToken, separatorStack[^1]))
                {
                    // If the separator matches the top of the stack, pop it off. If the stack is now empty, that means
                    // we just hit the closing separator of the entire expression, so we add the last entry and exit
                    separatorStack.RemoveAt(separatorStack.Count - 1);
                    
                    if (IsTokenInValuesOrTypes(values, types, currentToken) && separatorStack.Count == 0)
                    {
                        return i;
                    }
                }
                else
                {
                    throw new ParserUnexpectedClosingSeparatorException(currentToken);
                }
            }
        }

        return -1;
    }

    private static bool IsTokenInValuesOrTypes(List<string>? values, List<Consts.TokenTypes>? types, Token token)
    {
        return IsTokenInCollection(values, token.Value) || IsTokenInCollection(types, token.Type);
    }

    private static bool IsTokenInCollection<T>(List<T>? collection, T token)
    {
        return collection is not null && collection.Contains(token);
    }

    private static bool DoesTokenMatchSeparator(Token token, string separator)
    {
        return Consts.MatchingSeparatorsMap[token.Value] == separator;
    }

    public static int GetOperatorIndex(List<Token> tokens)
    {
        // We want to prioritize:
        // 1. operators in separators over operators outside of separators
        // 2. higher priority operators over lower priority operators
        // 3. operators earlier in the expression over operators later in the expression
        // 
        // Because we are building a tree, the lowest priority should be identified first.  When we evaluate, we
        // evaluate leaves first and then work our way up.
        //
        // To do each of these things:
        // 1. We ignore operators in separators in this function because we will ultimately run the parser again
        // for the subexpression within the separators
        // 2. Operators are given a value, the higher the value, the lower the priority of the operator.  So we're
        // actually looking for the highest value operator when checking.
        // 3. We evaluate the expression from right to left
        
        (int Priority, int Index) lowest = (-1, -1);
        
        List<string> separatorStack = [];
        
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var currentToken = tokens[i];
            if (currentToken.Type == Consts.TokenTypes.Operator && separatorStack.Count == 0)
            {
                lowest = UpdateOperatorPriorityAndIndexForOperator(lowest, currentToken.Value, i);
            }
            else if (Consts.ClosingSeparators.Contains(currentToken.Value))
            {
                separatorStack.Add(currentToken.Value);
            } else if (Consts.OpeningSeparators.Contains(currentToken.Value))
            {
                if (DoesTokenMatchSeparator(currentToken, separatorStack[^1]))
                {
                    // If the separator matches the top of the stack, pop it off. If the stack is now empty, that means
                    // we just hit the closing separator of the entire expression, so we add the last entry and exit
                    separatorStack.RemoveAt(separatorStack.Count - 1);
                    
                    if (currentToken.Type == Consts.TokenTypes.Operator && separatorStack.Count == 0)
                    {
                        lowest = UpdateOperatorPriorityAndIndexForOperator(lowest, currentToken.Value, i);
                    }
                }
                else
                {
                    throw new ParserUnexpectedClosingSeparatorException(currentToken);
                }
            }
        }
        
        return lowest.Index;
    }

    private static (int Priority, int Index) UpdateOperatorPriorityAndIndexForOperator(
        (int Priority, int Index) current,
        string operatorString,
        int index)
    {
        var priority = Array.FindIndex(Consts.Operators, e => e == operatorString);
        return (priority != -1 && priority > current.Priority) ? (priority, index) : current;
    }
    
    public static (Instruction? Left, Instruction? Right) ParseLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = index > 0 ? Instruction.Parse(tokens.GetRange(0, index)) : null;
        var right = index < tokens.Count - 1 ? 
            Instruction.Parse(tokens.GetRange(index + 1, tokens.Count - index - 1)) : 
            null;

        return (left, right);
    }
    
    public static (int Count, List<Instruction> Values) ParseEntriesWithinSeparator(
        List<Token> tokens, 
        List<string> separators
    )
    {
        // Early return for no tokens present
        if (tokens.Count == 0) return (0, []);
        
        var closingSeparator = Consts.MatchingSeparatorsMap[tokens[0].Value];
        var closingSeparatorIndex = GetTokenIndex(tokens, [closingSeparator]);
        var tokensWithinSeparator = tokens.GetRange(1, closingSeparatorIndex - 1);
        var results = GroupTokensWithSeparators(tokensWithinSeparator, separators);
        
        List<Instruction> values = [];
        foreach (var entry in results.Entries)
        {
            if (entry.Count > 0)
            {
                values.Add(Instruction.Parse(entry));
            }
        }

        return (results.Count + 2, values);
    }
    
    public static (int Count, List<Instruction> Values) ParseEntriesBetweenSeparatingCharacters(
        List<Token> tokens, 
        List<string> separators
    )
    {
        // Early return for no tokens present
        if (tokens.Count == 0) return (0, []);
        
        var results = GroupTokensWithSeparators(tokens, separators);
        
        List<Instruction> values = [];
        foreach (var entry in results.Entries)
        {
            values.Add(Instruction.Parse(entry));
        }

        return (results.Count, values);
    }
    
    private static (int Count, List<List<Token>> Entries) GroupTokensWithSeparators(
        List<Token> tokens, 
        List<string> separatingCharacters)
    {
        List<string> separatorStack = [];
        List<List<Token>> entries = [];
        List<Token> currentTokenSet = [];
        var totalTokenCount = 0;

        while (totalTokenCount < tokens.Count)
        {
            var currentToken = tokens[totalTokenCount];
            
            if (Consts.OpeningSeparators.Contains(currentToken.Value))
            {
                separatorStack.Add(currentToken.Value);
                currentTokenSet.Add(currentToken);
            }
            else if (Consts.ClosingSeparators.Contains(currentToken.Value))
            {
                if (DoesTokenMatchSeparator(currentToken, separatorStack[^1]))
                {
                    separatorStack.RemoveAt(separatorStack.Count - 1);
                    currentTokenSet.Add(currentToken);
                }
                else
                {
                    throw new ParserUnexpectedClosingSeparatorException(currentToken);
                }
            }
            else if (separatorStack.Count == 0 && separatingCharacters.Contains(currentToken.Value))
            {
                entries.Add(currentTokenSet);
                currentTokenSet = [];
            }
            else
            {
                currentTokenSet.Add(currentToken);
            }
            
            totalTokenCount++;
        }
        
        entries.Add(currentTokenSet);

        if (separatorStack.Count == 0)
        {
            return (totalTokenCount, entries);
        }
        else
        {
            throw new ParserMatchingSeparatorNotFoundException(tokens[0]);
        }
    }
}