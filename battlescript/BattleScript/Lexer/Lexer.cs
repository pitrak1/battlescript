using BattleScript.Tokens;

namespace BattleScript.Core;

public class Lexer
{
    string contents;
    int contentIndex;
    int lineIndex;
    int lineNumber;

    List<Token> tokens;

    public Lexer(string _contents)
    {
        contents = _contents;
        contentIndex = 0;
        lineIndex = 0;
        lineNumber = 0;
        tokens = new List<Token>();
    }

    public List<Token> Run()
    {
        while (contentIndex < contents.Length)
        {
            string nextCharacters = getNextCharacters();
            char nextCharacter = nextCharacters[0];

            if (nextCharacter == '\n')
            {
                contentIndex++;
                lineIndex++;
                lineIndex = 0;
            }
            else if (Consts.Whitespace.Contains(nextCharacter))
            {
                contentIndex++;
                lineIndex++;
            }
            else if (Consts.Digits.Contains(nextCharacter))
            {
                handleNumber();
            }
            else if (Consts.Quotes.Contains(nextCharacter))
            {
                handleString();
            }
            else if (Consts.Separators.Contains(nextCharacter))
            {
                handleSeparator(nextCharacter.ToString());
            }
            else if (Consts.StartingWordCharacters.Contains(nextCharacter))
            {
                handleWord();
            }
            else if (Consts.Operators.Contains(nextCharacters))
            {
                handleOperator(nextCharacters);
            }
            else if (Consts.Operators.Contains(nextCharacter.ToString()))
            {
                handleOperator(nextCharacter.ToString());
            }
            else if (nextCharacter == '=')
            {
                handleAssignment();
            }
            else if (nextCharacter == ';')
            {
                handleSemicolon();
            }
            else if (nextCharacters == "//")
            {
                handleComment();
            }
            else
            {
                throw new SystemException("Invalid character found");
            }
        }

        return tokens;
    }

    private void handleNumber()
    {
        // Get all following characters that are digits and periods
        string result = Utilities.GetCharactersUsingCollection(
            contents,
            contentIndex,
            Consts.NumberCharacters,
            CollectionType.Inclusive
        );

        Token token = new Token(Consts.TokenTypes.Number, result, lineNumber, lineIndex);
        tokens.Add(token);

        contentIndex += token.Value.Length;
        lineIndex += token.Value.Length;
    }

    private void handleString()
    {
        char startingQuote = contents[contentIndex];

        // Get all following characters until matching quote
        string result = Utilities.GetCharactersUsingCollection(
            contents,
            contentIndex + 1,
            new char[] { startingQuote },
            CollectionType.Exclusive
        );

        string finalString = startingQuote + result + startingQuote;

        Token token = new Token(Consts.TokenTypes.String, finalString, lineNumber, lineIndex);
        tokens.Add(token);
        contentIndex += token.Value.Length;
        lineIndex += token.Value.Length;
    }

    private void handleSeparator(string nextCharacter)
    {
        Token token = new Token(Consts.TokenTypes.Separator, nextCharacter, lineNumber, lineIndex);
        tokens.Add(token);
        contentIndex++;
        lineIndex++;
    }

    private void handleWord()
    {
        // Get all following characters that are letters, numbers, or _
        string result = Utilities.GetCharactersUsingCollection(
            contents,
            contentIndex,
            Consts.WordCharacters,
            CollectionType.Inclusive
        );

        Consts.TokenTypes type = Consts.TokenTypes.Identifier;
        if (Consts.Keywords.Contains(result))
        {
            type = Consts.TokenTypes.Keyword;
        }
        else if (Consts.Booleans.Contains(result))
        {
            type = Consts.TokenTypes.Boolean;
        }

        Token token = new Token(type, result, lineNumber, lineIndex);
        tokens.Add(token);
        contentIndex += token.Value.Length;
        lineIndex += token.Value.Length;
    }

    private void handleOperator(string operatorString)
    {
        Token token = new Token(Consts.TokenTypes.Operator, operatorString, lineNumber, lineIndex);
        tokens.Add(token);
        contentIndex += token.Value.Length;
        lineIndex += token.Value.Length;
    }

    private void handleAssignment()
    {
        Token token = new Token(Consts.TokenTypes.Assignment, "=", lineNumber, lineIndex);
        tokens.Add(token);
        contentIndex++;
        lineIndex++;
    }

    private void handleSemicolon()
    {
        Token token = new Token(Consts.TokenTypes.Semicolon, ";", lineNumber, lineIndex);
        tokens.Add(token);
        contentIndex++;
        lineIndex++;
    }

    private void handleComment()
    {
        // Get all following characters until newline
        string charactersUntilNextLine = Utilities.GetCharactersUsingCollection(
            contents,
            contentIndex,
            new char[] { '\n' },
            CollectionType.Exclusive
        );
        contentIndex += charactersUntilNextLine.Length;
        lineIndex += charactersUntilNextLine.Length;
    }

    private string getNextCharacters()
    {
        try
        {
            return contents.Substring(contentIndex, 2);
        }
        catch (ArgumentOutOfRangeException)
        {
            return contents.Substring(contentIndex);
        }
    }
}