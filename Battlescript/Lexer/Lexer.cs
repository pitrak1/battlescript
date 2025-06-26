namespace Battlescript;

public class Lexer(string input)
{
    private int _index;
    private int _line = 1;
    private int _column;

    private readonly List<Token> _tokens = [];

    public List<Token> Run()
    {
        while (_index < input.Length)
        {
            var (nextNextNextCharacter, nextNextCharacter, nextCharacter) = 
                LexerUtilities.GetNextThreeCharacters(input, _index);
            
            if (nextCharacter[0] == '\n')
            {
                HandleNewline();
            }
            else if (nextCharacter[0] == ' ')
            {
                _index++;
                _column++;
            } 
            else if (nextCharacter[0] == '.' && Consts.Digits.Contains(nextNextCharacter[0]))
            {
                HandleNumber();
            }
            else if (Consts.Digits.Contains(nextCharacter[0]))
            {
                HandleNumber();
            }
            else if (Consts.Quotes.Contains(nextCharacter[0]))
            {
                HandleString();
            }
            else if (Consts.Separators.Contains(nextCharacter[0]))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Separator, nextCharacter, _line, _column));
                _index++;
                _column++;
            }
            else if (Consts.StartingWordCharacters.Contains(nextCharacter[0]))
            {
                HandleWord();
            }
            else if (Consts.Operators.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter + nextNextCharacter + nextNextNextCharacter, _line, _column));
                _index += 3;
                _column += 3;
            }
            else if (Consts.Assignments.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Assignment, nextCharacter + nextNextCharacter + nextNextNextCharacter, _line, _column));
                _index += 3;
                _column += 3;
            }
            else if (Consts.Operators.Contains(nextCharacter + nextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter + nextNextCharacter, _line, _column));
                _index += 2;
                _column += 2;
            }
            else if (Consts.Assignments.Contains(nextCharacter + nextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Assignment, nextCharacter + nextNextCharacter, _line, _column));
                _index += 2;
                _column += 2;
            }
            else if (Consts.Operators.Contains(nextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter, _line, _column));
                _index++;
                _column++;
            }
            else if (Consts.Assignments.Contains(nextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Assignment, nextCharacter, _line, _column));
                _index++;
                _column++;
            }
            else if (nextCharacter[0] == '#')
            {
                HandleComment();
            }
            else
            {
                throw new LexerException(nextCharacter + nextNextCharacter + nextNextNextCharacter, _line, _column);
            }
        }
        
        return _tokens;
    }

    private void HandleNewline()
    {
        // We are assuming indent sizes of 4 spaces or 1 tab
        var indent = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index + 1,
            Consts.Indentations,
            CollectionType.Inclusive
        );
        
        var totalIndent = LexerUtilities.GetIndentValueFromIndentationString(indent);
                
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
        // This is so we can skip the negative sign and search for numbers
        var negativeSign = "";
        if (input[_index] == '-')
        {
            negativeSign = "-";
            _index++;
            _column++;
        }
        
        var numberCharacters = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index, 
            Consts.NumberCharacters, 
            CollectionType.Inclusive
        );
        
        var isFloat = numberCharacters.Any(c => c == '.');
        var type = isFloat ? Consts.TokenTypes.Float : Consts.TokenTypes.Integer;

        _tokens.Add(new Token(type, negativeSign + numberCharacters, _line, _column));
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
        else if (Consts.BuiltInFunctions.Contains(word))
        {
            type = Consts.TokenTypes.BuiltIn;
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