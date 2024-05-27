using BattleScript.Core;

namespace BattleScript.LexerNS;

public enum CollectionType
{
    Inclusive,
    Exclusive
}

public class LexerUtilities
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

    public static string GetNextCharacters(string contents, int contentIndex)
    {
        try
        {
            return contents.Substring(contentIndex, 2);
        }
        catch (ArgumentOutOfRangeException)
        {
            return contents.Substring(contentIndex);
        }
    }
}