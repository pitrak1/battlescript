using BattleScript.Tokens;

namespace BattleScript.Core;

public class Lexer
{
    public static List<Token> Run(string contents)
    {
        List<Token> tokens = new List<Token>();

        string[] lines = contents.Split('\n');

        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            string line = lines[lineNumber];
            HandleLine(tokens, line, lineNumber);
        }

        return tokens;
    }

    private static void HandleLine(List<Token> tokens, string line, int lineNumber)
    {
        int lineIndex = 0;
        while (lineIndex < line.Length)
        {
            Token token = HandleNextToken(line, lineIndex);

            switch (token.Type)
            {
                case Consts.TokenTypes.Comment:
                    return;
                case Consts.TokenTypes.Whitespace:
                    lineIndex++;
                    break;
                default:
                    token.SetDebugInfo(lineNumber, lineIndex);
                    tokens.Add(token);
                    lineIndex += token.Value.Length;
                    break;
            }
        }
    }

    private static Token HandleNextToken(string line, int lineIndex)
    {
        int charactersToFetch = Math.Min(line.Length - lineIndex, 2);
        string nextCharacters = line.Substring(lineIndex, charactersToFetch);

        if (Consts.Whitespace.Contains(nextCharacters[0]))
        {
            return new WhitespaceToken();
        }
        else if (Consts.Digits.Contains(nextCharacters[0]))
        {
            return HandleNumber(line, lineIndex);
        }
        else if (Consts.Quotes.Contains(nextCharacters[0]))
        {
            return HandleString(line, lineIndex);
        }
        else if (Consts.Separators.Contains(nextCharacters[0]))
        {
            return new SeparatorToken(nextCharacters[0].ToString());
        }
        else if (Consts.StartingWordCharacters.Contains(nextCharacters[0]))
        {
            return HandleWord(line, lineIndex);
        }
        else if (Consts.Operators.Contains(nextCharacters))
        {
            return new OperatorToken(nextCharacters);
        }
        else if (Consts.Operators.Contains(nextCharacters[0].ToString()))
        {
            return new OperatorToken(nextCharacters[0].ToString());
        }
        else if (nextCharacters[0] == '=')
        {
            return new AssignmentToken();
        }
        else if (nextCharacters[0] == ';')
        {
            return new SemicolonToken();
        }
        else if (nextCharacters == "//")
        {
            return new CommentToken();
        }
        else
        {
            throw new SystemException("Invalid character found");
        }
    }

    private static Token HandleNumber(string line, int lineIndex)
    {
        string result = LexerUtilities.GetLineWhileCharactersInCollection(line, lineIndex, Consts.NumberCharacters);
        return new NumberToken(result);
    }

    private static Token HandleString(string line, int lineIndex)
    {
        char startingQuote = line[lineIndex];
        lineIndex++;
        string result = LexerUtilities.GetLineUntilCharacterInCollection(line, lineIndex, new char[] { startingQuote });
        string finalString = startingQuote + result + startingQuote;
        return new StringToken(finalString);
    }

    private static Token HandleWord(string line, int lineIndex)
    {
        string result = LexerUtilities.GetLineWhileCharactersInCollection(line, lineIndex, Consts.WordCharacters);

        Consts.TokenTypes type = Consts.TokenTypes.Identifier;
        if (Consts.Keywords.Contains(result))
        {
            type = Consts.TokenTypes.Keyword;
        }
        else if (Consts.Booleans.Contains(result))
        {
            type = Consts.TokenTypes.Boolean;
        }

        return new Token(type, result);
    }
}