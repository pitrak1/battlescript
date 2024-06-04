namespace BattleScript.Core;

public enum CollectionType
{
    Inclusive,
    Exclusive
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
}