using System.Collections.Frozen;

namespace Battlescript;

public static class InstructionUtilities
{
    private static readonly FrozenDictionary<string, int> OperatorPriority = new Dictionary<string, int>
    {
        { "or", 0 },
        { "and", 1 },
        { "not", 2 },
        { "not in", 3 },
        { "in", 4 },
        { "is not", 5 },
        { "is", 6 },
        { "<=", 7 },
        { "<", 8 },
        { ">=", 9 },
        { ">", 10 },
        { "!=", 11 },
        { "==", 12 },
        { "-", 13 },
        { "+", 14 },
        { "%", 15 },
        { "//", 16 },
        { "/", 17 },
        { "*", 18 },
        { "-1", 19 },
        { "+1", 20 },
        { "**", 21 }
    }.ToFrozenDictionary();
    
    private static readonly HashSet<Consts.TokenTypes> BinaryPrecedingTypes =
    [
        Consts.TokenTypes.String,
        Consts.TokenTypes.Numeric,
        Consts.TokenTypes.FormattedString,
        Consts.TokenTypes.Constant,
        Consts.TokenTypes.Identifier,
    ];

    private static readonly HashSet<string> BinaryPrecedingValues = [")", "]"];

    public static int GetTokenIndex(
        List<Token> tokens,
        List<string>? values = null,
        List<Consts.TokenTypes>? types = null,
        bool respectLambda = true)
    {
        bool MatchesValueOrType(Token t) =>
            (values?.Contains(t.Value) ?? false) || (types?.Contains(t.Type) ?? false);

        var depth = 0;
        var lambdaDepth = 0;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            var wasAtRoot = depth == 0;

            if (Consts.OpeningBrackets.Contains(token.Value))
                depth++;
            else if (Consts.ClosingBrackets.Contains(token.Value))
                depth--;

            // Track lambda context - when we see "lambda", the next colon belongs to it
            if (respectLambda && token is { Type: Consts.TokenTypes.Keyword, Value: "lambda" })
                lambdaDepth++;

            // Colon after lambda ends the parameter list but is part of the lambda
            if (respectLambda && lambdaDepth > 0 && token.Value == ":")
            {
                lambdaDepth--;
                continue;  // Don't match this colon
            }

            var isAtRoot = depth == 0;

            if ((wasAtRoot || isAtRoot) && lambdaDepth == 0 && MatchesValueOrType(token))
            {
                return i;
            }
        }

        return -1;
    }

    public static int GetLowestPriorityOperatorIndex(List<Token> tokens)
    {
        // We want to prioritize:
        // 1. operators in separators over operators outside of separators
        // 2. higher priority operators over lower priority operators
        // 3. operators earlier in the expression over operators later in the expression
        //
        // Because we are building a tree, the lowest priority should be identified first.
        // When we evaluate, we evaluate leaves first and then work our way up.
        //
        // To do each of these things:
        // 1. We ignore operators in separators (they'll be parsed in subexpressions)
        // 2. Higher value in OperatorPriority = lower priority, so we find max value
        // 3. We evaluate right to left

        var depth = 0;
        (int Priority, int Index) lowest = (Int32.MaxValue, -1);

        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];

            if (Consts.ClosingBrackets.Contains(token.Value))
                depth++;
            else if (Consts.OpeningBrackets.Contains(token.Value))
                depth--;

            if (depth == 0 && token.Type == Consts.TokenTypes.Operator)
            {
                lowest = GetLowerPriorityOperator(tokens, lowest, token.Value, i);
            }
        }

        return lowest.Index;
    }
    
    private static (int Priority, int Index) GetLowerPriorityOperator(
        List<Token> tokens,
        (int Priority, int Index) current,
        string operatorValue,
        int index)
    {
        var effectiveOperator = IsUnaryOperator(tokens, index) ? operatorValue + "1" : operatorValue;

        if (!OperatorPriority.TryGetValue(effectiveOperator, out var priority))
        {
            return current;
        }

        return priority < current.Priority ? (priority, index) : current;
    }
    
    private static bool IsUnaryOperator(List<Token> tokens, int index)
    {
        var token = tokens[index];
        if (token.Value != "+" && token.Value != "-")
        {
            return false;
        }

        if (index == 0)
        {
            return true;
        }

        var previous = tokens[index - 1];
        return !BinaryPrecedingTypes.Contains(previous.Type) && !BinaryPrecedingValues.Contains(previous.Value);
    }

    public static (Instruction? Left, Instruction? Right) ParseLeftAndRightAroundIndex(List<Token> tokens, int index)
    {
        var left = index > 0 ? InstructionFactory.Create(tokens[..index]) : null;
        var right = index < tokens.Count - 1 ? InstructionFactory.Create(tokens[(index + 1)..]) : null;
        return (left, right);
    }

    public static List<Instruction?> ParseEntriesBetweenDelimiters(List<Token> tokens, List<string> delimiters)
    {
        if (tokens.Count == 0) return [];
        if (delimiters.Count == 0) return [InstructionFactory.Create(tokens)];

        var depth = 0;
        var lambdaDepth = 0;
        List<Token> currentTokenGroup = [];
        List<Instruction?> instructions = [];

        foreach (var token in tokens)
        {
            if (Consts.OpeningBrackets.Contains(token.Value))
                depth++;
            else if (Consts.ClosingBrackets.Contains(token.Value))
                depth--;

            // Track lambda context - when we see "lambda", the next colon belongs to it
            if (token is { Type: Consts.TokenTypes.Keyword, Value: "lambda" })
                lambdaDepth++;

            // If we're in a lambda context and see a colon, it ends the lambda parameters
            if (lambdaDepth > 0 && token.Value == ":")
            {
                lambdaDepth--;
                currentTokenGroup.Add(token);
                continue;
            }

            if (depth == 0 && lambdaDepth == 0 && delimiters.Contains(token.Value))
            {
                instructions.Add(InstructionFactory.Create(currentTokenGroup));
                currentTokenGroup = [];
            }
            else
            {
                currentTokenGroup.Add(token);
            }
        }

        instructions.Add(InstructionFactory.Create(currentTokenGroup));
        return instructions;
    }

    public static List<Token> GetGroupedTokensAtStart(List<Token> tokens)
    {
        if (!Consts.OpeningBrackets.Contains(tokens[0].Value))
        {
            return [];
        }

        var closingBracket = Consts.MatchingBracketsMap[tokens[0].Value];
        var closingIndex = GetTokenIndex(tokens, [closingBracket]);
        return tokens[1..closingIndex];
    }
}