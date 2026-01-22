namespace Battlescript;

public static class LexerUtilities
{
    public static (int Length, string Result) GetNextCharactersWhile(
        string input,
        int index,
        Func<char, bool> predicate,
        bool allowEscapes = false)
    {
        var startingIndex = index;
        var result = "";
        while (index < input.Length)
        {
            if (allowEscapes && input[index] == '\\')
            {
                result += input[index + 1].ToString();
                index += 2;
            }
            else if (!predicate(input[index]))
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
    }

    public static (int Length, string Result) GetStringWithEscapes(string input, int index)
    {
        var quoteChar = input[index];
        var result = GetNextCharactersWhile(input, index + 1, c => c != quoteChar, allowEscapes: true);

        if (index + result.Length + 1 >= input.Length)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "EOL while scanning string literal");
        }

        return result;
    }
    
    public static (int Length, string Result) GetIndentValue(string input, int index)
    {
        var indentString = GetNextCharactersWhile(
            input,
            index,
            Consts.IsIndentation
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