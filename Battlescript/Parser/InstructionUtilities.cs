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
        List<Consts.TokenTypes>? types = null)
    {
        bool MatchesValueOrType(Token t) =>
            (values?.Contains(t.Value) ?? false) || (types?.Contains(t.Type) ?? false);

        var tracker = new BracketTracker();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            var wasAtRoot = tracker.IsAtRootLevel;
            tracker.Update(token);
            var isAtRoot = tracker.IsAtRootLevel;

            if ((wasAtRoot || isAtRoot) && MatchesValueOrType(token))
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

        var tracker = new BracketTracker();
        (int Priority, int Index) lowest = (Int32.MaxValue, -1);

        for (var i = tokens.Count - 1; i >= 0; i--)
        {
            var token = tokens[i];
            tracker.Update(token, reverse: true);

            if (tracker.IsAtRootLevel && token.Type == Consts.TokenTypes.Operator)
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

        var tracker = new BracketTracker();
        List<Token> currentTokenGroup = [];
        List<Instruction?> instructions = [];

        foreach (var token in tokens)
        {
            tracker.Update(token);

            if (tracker.IsAtRootLevel && delimiters.Contains(token.Value))
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

    private class BracketTracker
    {
        private readonly Stack<string> _stack = [];

        public bool IsAtRootLevel => _stack.Count == 0;

        public void Update(Token token, bool reverse = false)
        {
            var stackCollection = reverse ? Consts.ClosingBrackets : Consts.OpeningBrackets;
            var unstackCollection = reverse ? Consts.OpeningBrackets : Consts.ClosingBrackets;

            if (stackCollection.Contains(token.Value))
            {
                _stack.Push(token.Value);
            }
            else if (unstackCollection.Contains(token.Value))
            {
                if (_stack.Count > 0 && Consts.MatchingBracketsMap[token.Value] == _stack.Peek())
                {
                    _stack.Pop();
                }
                else
                {
                    throw new ParserUnexpectedClosingSeparatorException(token);
                }
            }
        }
    }
}