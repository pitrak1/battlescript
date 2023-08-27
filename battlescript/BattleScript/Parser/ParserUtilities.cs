using System.Diagnostics;
using BattleScript.Tokens;

namespace BattleScript.Core;

public class ParserUtilities
{
    public static int GetAssignmentOperatorIndex(List<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == Consts.TokenTypes.Assignment)
            {
                return i;
            }
        }

        return -1;
    }

    public static int GetMathematicalOperatorIndex(List<Token> tokens)
    {
        int lowestOperatorPriority = Consts.Operators.Length;
        int lowestOperatorLocation = tokens.Count;

        List<string> separatorStack = new List<string>();

        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            var currentValue = tokens[i].Value;
            string matchingSeparator;
            Consts.MatchingSeparatorsMap.TryGetValue(currentValue, out matchingSeparator);
            if (Consts.ClosingSeparators.Contains(currentValue))
            {
                separatorStack.Add(currentValue);
            }
            else if (separatorStack.Count > 0 && separatorStack[^1] == matchingSeparator)
            {
                separatorStack.RemoveAt(separatorStack.Count - 1);
            }
            else if (tokens[i].Type == Consts.TokenTypes.Operator && separatorStack.Count == 0)
            {
                int currentOperatorPriority = Array.FindIndex(Consts.Operators, element => element == currentValue);
                if (currentOperatorPriority != -1 && currentOperatorPriority < lowestOperatorPriority)
                {
                    lowestOperatorPriority = currentOperatorPriority;
                    lowestOperatorLocation = i;
                }
            }
        }

        if (lowestOperatorPriority != Consts.Operators.Length)
        {
            return lowestOperatorLocation;
        }
        else
        {
            return -1;
        }
    }

    public static List<List<Token>> ParseUntilMatchingSeparator(List<Token> tokens, List<string> separatingCharacters)
    {
        Debug.Assert(Consts.OpeningSeparators.Contains(tokens[0].Value));

        if (tokens.Count == 0)
        {
            return new List<List<Token>>();
        }

        List<string> separatorStack = new List<string>();
        List<List<Token>> entries = new List<List<Token>>();
        List<Token> currentTokenSet = new List<Token>();

        foreach (Token token in tokens)
        {
            if (Consts.OpeningSeparators.Contains(token.Value))
            {
                separatorStack.Add(token.Value);
                if (separatorStack.Count > 1)
                {
                    currentTokenSet.Add(token);
                }
            }
            else if (Consts.ClosingSeparators.Contains(token.Value))
            {
                string matchingValue = Consts.MatchingSeparatorsMap[token.Value];
                if (matchingValue == separatorStack[^1])
                {
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
                    throw new SystemException("Unexpected closing separator");
                }
            }
            else if (separatorStack.Count == 1 && separatingCharacters.Contains(token.Value))
            {
                entries.Add(currentTokenSet);
                currentTokenSet = new List<Token>();
            }
            else
            {
                currentTokenSet.Add(token);
            }
        }

        return entries;
    }

    public static int GetTokenLengthOfEntries(List<List<Token>> entries)
    {
        int total = 0;
        foreach (List<Token> entry in entries)
        {
            total += entry.Count;
        }
        // Add breaking characters and starting and ending seaprators
        total += entries.Count + 1;
        return total;
    }

    public static bool BlockContainsSemicolon(List<Token> tokens, int tokenIndex)
    {
        while (tokenIndex < tokens.Count)
        {
            switch (tokens[tokenIndex].Value)
            {
                case "}":
                    return false;
                case ";":
                    return true;
                default:
                    tokenIndex++;
                    break;
            }
        }
        return false;
    }

    public static bool BlockContainsSemicolonReverse(List<Token> tokens, int tokenIndex)
    {
        while (tokenIndex >= 0)
        {
            switch (tokens[tokenIndex].Value)
            {
                case "{":
                    return false;
                case ";":
                    return true;
                default:
                    tokenIndex--;
                    break;
            }
        }
        return false;
    }
}