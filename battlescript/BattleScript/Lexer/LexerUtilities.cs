namespace BattleScript.LexerNS;

public class LexerUtilities
{
    public static string GetLineUntilCharacterInCollection(string contents, int contentIndex, char[] collection)
    {
        string resultString = "";
        while (contentIndex < contents.Length)
        {
            char currentCharacter = contents[contentIndex];
            if (collection.Contains(currentCharacter))
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

    public static string GetLineWhileCharactersInCollection(string contents, int contentIndex, char[] collection)
    {
        string resultString = "";
        while (contentIndex < contents.Length)
        {
            char currentCharacter = contents[contentIndex];
            if (collection.Contains(currentCharacter))
            {
                resultString += currentCharacter.ToString();
                contentIndex++;
            }
            else
            {
                break;
            }
        }

        return resultString;
    }
}