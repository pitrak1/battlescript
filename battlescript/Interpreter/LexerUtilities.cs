namespace BattleScript; 

public class LexerUtilities {
    public static string GetLineUntilCharacterInCollection(string line, int lineIndex, char[] collection) {
        string resultString = "";
        while (lineIndex < line.Length) {
            char currentCharacter = line[lineIndex];
            if (collection.Contains(currentCharacter)) {
                break;
            } else {
                resultString += currentCharacter.ToString();
                lineIndex++;
            }
        }

        return resultString;
    }

    public static string GetLineWhileCharactersInCollection(string line, int lineIndex, char[] collection) {
        string resultString = "";
        while (lineIndex < line.Length) {
            char currentCharacter = line[lineIndex];
            if (collection.Contains(currentCharacter)) {
                resultString += currentCharacter.ToString();
                lineIndex++;
            } else {
                break;
            }
        }

        return resultString;
    }
}