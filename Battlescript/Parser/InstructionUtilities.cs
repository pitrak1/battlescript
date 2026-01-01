using System.Diagnostics;

namespace Battlescript;

public static class InstructionUtilities
{
    public static int GetTokenIndex(
        List<Token> tokens, 
        List<string>? values = null, 
        List<Consts.TokenTypes>? types = null
    )
    {
        List<string> bracketStack = [];
        
        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];
            if (IsTokenInValuesOrTypes(values, types, currentToken) && bracketStack.Count == 0)
            {
                return i;
            }
            else if (Consts.OpeningBrackets.Contains(currentToken.Value))
            {
                bracketStack.Add(currentToken.Value);
            } else if (Consts.ClosingBrackets.Contains(currentToken.Value))
            {
                if (bracketStack.Count > 0 && DoesTokenMatchPreviousOpeningBracket(currentToken, bracketStack[^1]))
                {
                    bracketStack.RemoveAt(bracketStack.Count - 1);
                    
                    if (IsTokenInValuesOrTypes(values, types, currentToken) && bracketStack.Count == 0)
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

    private static bool DoesTokenMatchPreviousOpeningBracket(Token token, string bracket)
    {
        return Consts.MatchingBracketsMap[token.Value] == bracket;
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
        
        List<string> bracketStack = [];
        
        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var currentToken = tokens[i];
            if (currentToken.Type == Consts.TokenTypes.Operator && bracketStack.Count == 0)
            {
                lowest = UpdateOperatorPriorityAndIndexForOperator(tokens, lowest, currentToken.Value, i);
            }
            else if (Consts.ClosingBrackets.Contains(currentToken.Value))
            {
                bracketStack.Add(currentToken.Value);
            } else if (Consts.OpeningBrackets.Contains(currentToken.Value))
            {
                if (DoesTokenMatchPreviousOpeningBracket(currentToken, bracketStack[^1]))
                {
                    // If the separator matches the top of the stack, pop it off. If the stack is now empty, that means
                    // we just hit the closing separator of the entire expression, so we add the last entry and exit
                    bracketStack.RemoveAt(bracketStack.Count - 1);
                    
                    if (currentToken.Type == Consts.TokenTypes.Operator && bracketStack.Count == 0)
                    {
                        lowest = UpdateOperatorPriorityAndIndexForOperator(tokens, lowest, currentToken.Value, i);
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

    private static bool IsUnaryOperator(List<Token> tokens, int index)
    {
        // Binary operators will separate two operands which can be:
        // 1. a literal string, numeric, bool, whatever
        // 2. a variable
        // 3. an index (ending in ])
        // 4. a function call (ending in ))
        
        // If it's ANYTHING else, it's a unary operator
        
        var currentToken = tokens[index];
        if (currentToken.Value != "+" && currentToken.Value != "-")
        {
            return false;
        }

        if (index > 0)
        {
            var previousToken = tokens[index - 1];
            var binaryTypes = new List<Consts.TokenTypes>() {
                Consts.TokenTypes.String,
                Consts.TokenTypes.Numeric,
                Consts.TokenTypes.FormattedString,
                Consts.TokenTypes.Constant,
                Consts.TokenTypes.Identifier,
            };
            var binaryValues = new List<string>() { "]", ")" };
            if (binaryTypes.Contains(previousToken.Type) || 
                binaryValues.Contains(previousToken.Value))
            {
                return false;
            }
        }

        return true;
    }

    private static (int Priority, int Index) UpdateOperatorPriorityAndIndexForOperator(
        List<Token> tokens,
        (int Priority, int Index) current,
        string operatorString,
        int index)
    {
        if (IsUnaryOperator(tokens, index))
        {
            operatorString += "1";
        }
        
        var priority = Array.FindIndex(Consts.OperatorPriority, e => e == operatorString);
        return (priority != -1 && priority > current.Priority) ? (priority, index) : current;
    }
    
    public static (Instruction? Left, Instruction? Right) ParseLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = index > 0 ? InstructionFactory.Create(tokens.GetRange(0, index)) : null;
        var right = index < tokens.Count - 1 ? 
            InstructionFactory.Create(tokens.GetRange(index + 1, tokens.Count - index - 1)) : 
            null;

        return (left, right);
    }
    
    public static List<Instruction?> ParseEntriesBetweenDelimiters(
        List<Token> tokens, 
        List<string> delimiters
    )
    {
        // Early return for no tokens present
        if (tokens.Count == 0) return [];
        
        var results = GroupTokensWithBrackets(tokens, delimiters);
        
        List<Instruction?> values = [];
        foreach (var entry in results)
        {
            values.Add(InstructionFactory.Create(entry));
        }

        return values;
    }
    
    private static List<List<Token>> GroupTokensWithBrackets(
        List<Token> tokens, 
        List<string> delimiters)
    {
        List<string> bracketStack = [];
        List<List<Token>> entries = [];
        List<Token> currentTokenSet = [];


        foreach (var token in tokens)
        {
            if (Consts.OpeningBrackets.Contains(token.Value))
            {
                bracketStack.Add(token.Value);
                currentTokenSet.Add(token);
            }
            else if (Consts.ClosingBrackets.Contains(token.Value))
            {
                if (DoesTokenMatchPreviousOpeningBracket(token, bracketStack[^1]))
                {
                    bracketStack.RemoveAt(bracketStack.Count - 1);
                    currentTokenSet.Add(token);
                }
                else
                {
                    throw new ParserUnexpectedClosingSeparatorException(token);
                }
            }
            else if (bracketStack.Count == 0 && delimiters.Contains(token.Value))
            {
                entries.Add(currentTokenSet);
                currentTokenSet = [];
            }
            else
            {
                currentTokenSet.Add(token);
            }
        }
        
        entries.Add(currentTokenSet);

        if (bracketStack.Count == 0)
        {
            return entries;
        }
        else
        {
            throw new ParserMatchingSeparatorNotFoundException(tokens[0]);
        }
    }

    public static List<Token> GetGroupedTokensAtStart(List<Token> tokens)
    {
        if (Consts.OpeningBrackets.Contains(tokens[0].Value))
        {
            var closingBracket = Consts.MatchingBracketsMap[tokens[0].Value];
            var closingBracketIndex = GetTokenIndex(tokens, [closingBracket]);
            return tokens.GetRange(1, closingBracketIndex - 1);
        }
        else
        {
            return [];
        }
    }
}