namespace BattleScript; 

public class LexerUtilities {
    public static string GetLineUntilCharacters(string line, int lineIndex, char[] breakingCharacters) {
        string resultString = "";
        while (lineIndex < line.Length) {
            char currentCharacter = line[lineIndex];
            if (breakingCharacters.Contains(currentCharacter)) {
                break;
            } else {
                resultString += currentCharacter.ToString();
                lineIndex++;
            }
        }

        return resultString;
    }
}