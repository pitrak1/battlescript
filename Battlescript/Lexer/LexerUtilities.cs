namespace Battlescript;

// Inclusive would be a search that continues until a character is not in the collection, whereas
// an Exclusive would continue until a character is in the collection
public enum CollectionType
{
    Inclusive,
    Exclusive
}

public static class LexerUtilities
{
    public static (int Length, string Result) GetNextCharactersInCollection(
        string input, 
        int index, 
        char[] collection, 
        CollectionType type, 
        bool escapeCharacters = false)
    {
        var startingIndex = index;
        var result = "";
        while (index < input.Length)
        {
            if (EscapeCharacterFound(input[index]))
            {
                AddEscapedCharacter();
            }
            else if (BreakingCharacterFound(input[index]))
            {
                break;
            }
            else
            {
                result += input[index].ToString();
                index++;
            }
        }

        return (index - startingIndex, result);

        bool EscapeCharacterFound(char current)
        {
            return escapeCharacters && current == '\\';
        }

        void AddEscapedCharacter()
        {
            result += input[index + 1].ToString();
            index += 2;
        }

        bool BreakingCharacterFound(char current)
        {
            // If collection type is Exclusive and the current character is in the collection or if
            // collection type is Inclusive and the current character is not in the collection
            return collection.Contains(current) == (type == CollectionType.Exclusive);
        }
    }

    public static int GetIndentValueFromIndentationString(string indentations)
    {
        var totalSpaces = 0;
        foreach (var indentChar in indentations)
        {
            switch (indentChar)
            {
                case ' ':
                    totalSpaces++;
                    break;
                case '\t':
                    totalSpaces += 4;
                    break;
            }
        }
        return  (int)MathF.Floor(totalSpaces / 4f);
    }
    
    public static (string, string, string) GetNextThreeCharacters(string input, int index)
    {
        var remainingCharacters = input.Length - index;
        var nextCharacter = remainingCharacters >= 1 ? input[index].ToString() : "";
        var nextNextCharacter = remainingCharacters >= 2 ? input[index + 1].ToString() : "";
        var nextNextNextCharacter = remainingCharacters >= 3 ? input[index + 2].ToString() : "";

        return (nextNextNextCharacter, nextNextCharacter, nextCharacter);
    }
}