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
        CollectionType type = CollectionType.Inclusive,
        bool includeEscapes = false)
    {
        var startingIndex = index;
        var result = "";
        while (index < input.Length)
        {
            if (includeEscapes && input[index] == '\\')
            {
                result += input[index + 1].ToString();
                index += 2;
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
        
        bool BreakingCharacterFound(char current)
        {
            // If collection type is Exclusive and the current character is in the collection or if
            // collection type is Inclusive and the current character is not in the collection
            return collection.Contains(current) == (type == CollectionType.Exclusive);
        }
    }

    public static (int Length, string Result) GetStringWithEscapes(string input, int index)
    {
        // This is searching for a quote character matching the character at index
        var result = GetNextCharactersInCollection(input, index + 1, [input[index]], CollectionType.Exclusive, true);
        
        if (index + result.Length + 1 >= input.Length)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "EOL while scanning string literal");
        }

        return result;
    }
    
    public static (int Length, string Result) GetIndentValue(string input, int index)
    {
        var indentString = GetNextCharactersInCollection(
            input, 
            index,
            Consts.Indentations
        );
        
        var totalSpaces = 0;
        foreach (var indentChar in indentString.Result)
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
        var result =  ((int)MathF.Floor(totalSpaces / 4f)).ToString();
        return (indentString.Length, result);
    }
}