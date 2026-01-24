using System.Text;

namespace Battlescript;

public static class LexerUtilities
{
    private const int TabWidthInSpaces = 4;
    public static (int Length, string Result) GetNextCharactersWhile(
        string input,
        int index,
        Func<char, bool> predicate,
        bool allowEscapes = false)
    {
        var startingIndex = index;
        var result = new StringBuilder();
        while (index < input.Length)
        {
            if (allowEscapes && input[index] == '\\')
            {
                result.Append(input[index + 1]);
                index += 2;
            }
            else if (!predicate(input[index]))
            {
                break;
            }
            else
            {
                result.Append(input[index]);
                index++;
            }
        }

        return (index - startingIndex, result.ToString());
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
                    totalSpaces += TabWidthInSpaces;
                    break;
            }
        }
        var result = ((int)MathF.Floor(totalSpaces / (float)TabWidthInSpaces)).ToString();
        return (indentString.Length, result);
    }
}