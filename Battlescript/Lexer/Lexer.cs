namespace Battlescript;

public class Lexer(string input)
{
    private int _index;
    private int _line;
    private int _column;

    private List<Token> _tokens = [];

    public List<Token> Run()
    {
        while (_index < input.Length)
        {
            var (nextThreeCharacters, nextTwoCharacters, nextCharacter) = 
                LexerUtilities.GetNextThreeCharacters(input, _index);

            if (nextCharacter == '\n')
            {
                HandleNewline();
            } 
            else if (nextCharacter == ' ')
            {
                _index++;
                _column++;
            } 
            else if (Consts.Digits.Contains(nextCharacter))
            {
                HandleNumber();
            }
            else if (Consts.Quotes.Contains(nextCharacter))
            {
                HandleString();
            }
            else if (Consts.Separators.Contains(nextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Separator, nextCharacter.ToString(), _line, _column));
                _index++;
                _column++;
            }
            else if (Consts.StartingWordCharacters.Contains(nextCharacter))
            {
                HandleWord();
            }
            else if (Consts.Operators.Contains(nextThreeCharacters))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextThreeCharacters, _line, _column));
                _index += 3;
                _column += 3;
            }
            else if (Consts.Operators.Contains(nextTwoCharacters))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextTwoCharacters, _line, _column));
                _index += 2;
                _column += 2;
            }
            else if (Consts.Operators.Contains(nextCharacter.ToString()))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter.ToString(), _line, _column));
                _index++;
                _column++;
            }
            else if (nextCharacter == '#')
            {
                HandleComment();
            }
            else
            {
                throw new Exception("Invalid character found");
            }
        }
        
        return _tokens;
    }

    private void HandleNewline()
    {
        // We are assuming indent sizes of 4 spaces or 1 tab
        var indent = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index,
            Consts.Indentations,
            CollectionType.Inclusive
        );

        var totalSpaces = 0;
        foreach (var indentChar in indent)
        {
            if (indentChar == ' ')
            {
                totalSpaces++;
            }
            else if (indentChar == '\t')
            {
                totalSpaces += 4;
            }
        }
        var totalIndent = (int)MathF.Floor(totalSpaces);
                
        // This is the number of characters in the indent plus the newline
        _index += indent.Length + 1;
        // We want to update these before we create the token because we want any errors to be associated to
        // the indents on the second line, not the return on the first
        _line++;
        _column = 0;
        _tokens.Add(new Token(Consts.TokenTypes.Newline, totalIndent.ToString(), _line, _column));
    }

    private void HandleNumber()
    {
        var numberCharacters = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index, 
            Consts.NumberCharacters, 
            CollectionType.Inclusive
        );
        _tokens.Add(new Token(Consts.TokenTypes.Number, numberCharacters, _line, _column));
        _index += numberCharacters.Length;
        _column += numberCharacters.Length;
    }

    private void HandleString()
    {
        var startingQuoteCollection = new [] { input[_index] };
        // Get all characters after the starting quote and until we find a matching quote
        var stringContents = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index + 1, 
            startingQuoteCollection, 
            CollectionType.Exclusive
        );
        
        _tokens.Add(new Token(Consts.TokenTypes.String, stringContents, _line, _column));
        _index += stringContents.Length + 2;
        _column += stringContents.Length + 2;
    }

    private void HandleWord()
    {
        var word = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index, 
            Consts.WordCharacters, 
            CollectionType.Inclusive
        );
        
        var type = Consts.TokenTypes.Identifier;
        if (Consts.Keywords.Contains(word))
        {
            type = Consts.TokenTypes.Keyword;
        }
        else if (Consts.Booleans.Contains(word))
        {
            type = Consts.TokenTypes.Boolean;
        }
        else if (Consts.Operators.Contains(word))
        {
            type = Consts.TokenTypes.Operator;
        }
        
        _tokens.Add(new Token(type, word, _line, _column));
        _index += word.Length;
        _column += word.Length;
    }

    private void HandleComment()
    {
        // Get all next characters up until the newline
        var comment = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index, 
            new [] { '\n' }, 
            CollectionType.Exclusive
        );
        _index += comment.Length;
        _column += comment.Length;
    }
}