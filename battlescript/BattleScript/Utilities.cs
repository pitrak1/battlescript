using BattleScript.Tokens;
using System.Diagnostics;

namespace BattleScript.Core;

public enum CollectionType
{
    Inclusive,
    Exclusive
}

public enum SearchDirection
{
    Forward,
    Reverse
}

public class Utilities
{
    public static string GetCharactersUsingCollection(
        string contents,
        int contentIndex,
        char[] collection,
        CollectionType type
    )
    {
        string resultString = "";
        while (contentIndex < contents.Length)
        {
            char currentCharacter = contents[contentIndex];
            if (collection.Contains(currentCharacter) == (type == CollectionType.Exclusive))
            {
                break;
            }
            else
            {
                resultString += currentCharacter.ToString();
                contentIndex++;
            }
        }

        return resultString;
    }

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
        int lowestOperatorPriority = -1;
        int lowestOperatorLocation = -1;

        List<string> separatorStack = new List<string>();

        // Loop backwards so we evaluate from front to back
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            string matchingSeparatorForCurrentToken =
                Consts.MatchingSeparatorsMap.ContainsKey(tokens[i].Value) ?
                Consts.MatchingSeparatorsMap[tokens[i].Value] :
                "";

            // Token is a new separator we need to add to the stack 
            if (Consts.ClosingSeparators.Contains(tokens[i].Value))
            {
                separatorStack.Add(tokens[i].Value);
            }
            // Token is the matching separator for the separator on top of the stack
            else if (
                separatorStack.Count > 0 &&
                separatorStack[^1] == matchingSeparatorForCurrentToken
            )
            {
                separatorStack.RemoveAt(separatorStack.Count - 1);
            }
            // Token is a mathematical operator not within matching separators
            else if (tokens[i].Type == Consts.TokenTypes.Operator && separatorStack.Count == 0)
            {
                // Because we want to find lower priority operators, the Operators const is in
                // mathematical priority descending order, making it priority order ascending
                // with regard to this function
                int currentOperatorPriority =
                    Array.FindIndex(Consts.Operators, element => element == tokens[i].Value);
                if (
                    currentOperatorPriority != -1 &&
                    currentOperatorPriority > lowestOperatorPriority
                )
                {
                    lowestOperatorPriority = currentOperatorPriority;
                    lowestOperatorLocation = i;
                }
            }
        }

        return lowestOperatorLocation;
    }

    public static (int Count, List<List<Token>> Result) ParseUntilMatchingSeparator(List<Token> tokens, List<string> separatingCharacters)
    {
        if (tokens.Count == 0)
        {
            return (Count: 0, Result: new List<List<Token>>());
        }

        Debug.Assert(Consts.OpeningSeparators.Contains(tokens[0].Value));

        List<string> separatorStack = new List<string>();
        List<List<Token>> entries = new List<List<Token>>();
        List<Token> currentTokenSet = new List<Token>();
        int count = 0;

        foreach (Token token in tokens)
        {
            count++;
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

        if (entries.Count == 1 && entries[0].Count == 0)
        {
            return (Count: 0, Result: new List<List<Token>>());
        }

        return (Count: count, Result: entries);
    }

    public static bool AtStartOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        if (tokens[tokenIndex].Value != "{")
        {
            return false;
        }

        int nestCount = 0;

        while (tokenIndex < tokens.Count)
        {
            tokenIndex++;
            switch (tokens[tokenIndex].Value)
            {
                case "{":
                    if (AtStartOfCodeBlock(tokens, tokenIndex))
                    {
                        return true;
                    }
                    nestCount++;
                    break;
                case "}":
                    if (nestCount > 0)
                    {
                        nestCount--;
                        break;
                    }
                    else
                    {
                        return false;
                    }
                case ";":
                    return true;
            }
        }
        return false;
    }

    public static bool AtEndOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        if (tokens[tokenIndex].Value != "}")
        {
            return false;
        }

        int nestCount = 0;

        while (tokenIndex >= 0)
        {
            tokenIndex--;
            switch (tokens[tokenIndex].Value)
            {
                case "}":
                    if (AtEndOfCodeBlock(tokens, tokenIndex))
                    {
                        return true;
                    }
                    nestCount++;
                    break;
                case "{":
                    if (nestCount > 0)
                    {
                        nestCount--;
                        break;
                    }
                    else
                    {
                        return false;
                    }
                case ";":
                    return true;
            }
        }
        return false;
    }
}