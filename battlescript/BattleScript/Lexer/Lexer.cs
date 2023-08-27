using BattleScript.Tokens;
using BattleScript.Core;

namespace BattleScript.LexerNS;

public class Lexer
{
    string contents = "";
    int contentIndex = 0;
    int lineIndex = 0;
    int lineNumber = 0;

    List<Token> tokens = new List<Token>();

    public List<Token> Run(string _contents)
    {
        contents = _contents;
        contentIndex = 0;
        lineIndex = 0;
        lineNumber = 0;
        tokens = new List<Token>();

        while (contentIndex < contents.Length)
        {
            string nextCharacters = getNextCharacters(contents, contentIndex);
            char nextCharacter = nextCharacters[0];

            if (nextCharacter == '\n')
            {
                moveToNextLine();
            }
            else if (Consts.Whitespace.Contains(nextCharacter))
            {
                moveForwardOnce();
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

    private static string getNextCharacters(string contents, int contentIndex)
    {
        try
        {
            return contents.Substring(contentIndex, 2);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return contents.Substring(contentIndex);
        }
    }

    private void handleNumber()
    {
        string result = LexerUtilities.GetLineWhileCharactersInCollection(contents, contentIndex, Consts.NumberCharacters);

        Token token = new NumberToken(result, lineNumber, lineIndex);
        tokens.Add(token);

        moveForward(token.Value.Length);
    }

    private void handleString()
    {
        char startingQuote = contents[contentIndex];
        string result = LexerUtilities.GetLineUntilCharacterInCollection(contents, contentIndex + 1, new char[] { startingQuote });
        string finalString = startingQuote + result + startingQuote;

        Token token = new StringToken(finalString, lineNumber, lineIndex);
        tokens.Add(token);

        moveForward(token.Value.Length);
    }

    private void handleSeparator(string nextCharacter)
    {
        Token token = new SeparatorToken(nextCharacter, lineNumber, lineIndex);
        tokens.Add(token);

        moveForwardOnce();
    }

    private void handleWord()
    {
        string result = LexerUtilities.GetLineWhileCharactersInCollection(contents, contentIndex, Consts.WordCharacters);

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

        moveForward(token.Value.Length);
    }

    private void handleOperator(string operatorString)
    {
        Token token = new OperatorToken(operatorString, lineNumber, lineIndex);
        tokens.Add(token);

        moveForward(token.Value.Length);
    }

    private void handleAssignment()
    {
        Token token = new AssignmentToken(lineNumber, lineIndex);
        tokens.Add(token);

        moveForwardOnce();
    }

    private void handleSemicolon()
    {
        Token token = new SemicolonToken(lineNumber, lineIndex);
        tokens.Add(token);

        moveForwardOnce();
    }

    private void handleComment()
    {
        string charactersUntilNextLine = LexerUtilities.GetLineUntilCharacterInCollection(contents, contentIndex, new char[] { '\n' });

        moveForward(charactersUntilNextLine.Length);
    }

    private void moveToNextLine()
    {
        lineNumber++;
        lineIndex = 0;
        contentIndex++;
    }

    private void moveForward(int value)
    {
        lineIndex += value;
        contentIndex += value;
    }

    private void moveForwardOnce()
    {
        moveForward(1);
    }
}