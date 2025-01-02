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
    public static string GetNextCharactersInCollection(string input, int index, char[] collection, CollectionType type)
    {
        var result = "";
        while (index < input.Length)
        {
            var current = input[index];
            // If collection type is Exclusive and the current character is in the collection or if
            // collection type is Inclusive and the current character is not in the collection
            if (collection.Contains(current) == (type == CollectionType.Exclusive))
            {
                break;
            }
            else
            {
                result += current.ToString();
                index++;
            }
        }

        return result;
    }
    
    public static (string, string, char) GetNextThreeCharacters(string input, int index)
    {
        var remainingCharacters = input.Length - index;

        if (remainingCharacters <= 0)
        {
            return ("", "", ' ');
        }

        var threeCharacters = input.Substring(index, (int)MathF.Min(remainingCharacters, 3));
        var twoCharacters = input.Substring(index, (int)MathF.Min(remainingCharacters, 2));
        var character = input[index];
        
        return (threeCharacters, twoCharacters, character);
    }
}