namespace Battlescript;

public class Lexer(string input, string? fileName = null)
{
    private int _index;
    private int _line = 1;
    private string _lineContents = "";

    private readonly List<Token> _tokens = [];

    public List<Token> Run()
    {
        _lineContents = GetLineContents();
        
        while (_index < input.Length)
        {
            var (nextNextNextCharacter, nextNextCharacter, nextCharacter) = 
                LexerUtilities.GetNextThreeCharacters(input, _index);
            
            if (nextCharacter[0] == '\n')
            {
                HandleNewline(true);
            }
            else if (nextCharacter[0] == '\t')
            {
                HandleNewline(false);
            }
            else if (nextCharacter[0] == ' ')
            {
                _index++;
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
                _tokens.Add(new Token(Consts.TokenTypes.Separator, nextCharacter, _line, fileName, _lineContents));
                _index++;
            }
            else if (Consts.StartingWordCharacters.Contains(nextCharacter[0]))
            {
                HandleWord();
            }
            else if (Consts.Operators.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter + nextNextCharacter + nextNextNextCharacter, _line, fileName, _lineContents));
                _index += 3;
            }
            else if (Consts.Assignments.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Assignment, nextCharacter + nextNextCharacter + nextNextNextCharacter, _line, fileName, _lineContents));
                _index += 3;
            }
            else if (Consts.Operators.Contains(nextCharacter + nextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter + nextNextCharacter, _line, fileName, _lineContents));
                _index += 2;
            }
            else if (Consts.Assignments.Contains(nextCharacter + nextNextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Assignment, nextCharacter + nextNextCharacter, _line, fileName, _lineContents));
                _index += 2;
            }
            else if (Consts.Operators.Contains(nextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Operator, nextCharacter, _line, fileName, _lineContents));
                _index++;
            }
            else if (Consts.Assignments.Contains(nextCharacter))
            {
                _tokens.Add(new Token(Consts.TokenTypes.Assignment, nextCharacter, _line, fileName, _lineContents));
                _index++;
            }
            else if (nextCharacter[0] == '#')
            {
                HandleComment();
            }
            else
            {
                throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "invalid syntax");
            }
        }
        
        return _tokens;

        string GetLineContents()
        {
            if (input.IndexOf('\n', _index) != -1)
            {
                return input.Substring(_index, input.IndexOf('\n', _index) - _index);
            }
            else
            {
                return input.Substring(_index);
            }
        }
        
        void HandleNewline(bool includeNewline)
        {
            var startingIndex = _index + 1;
            if (!includeNewline)
            {
                startingIndex = _index;
            }
            
            // We are assuming indent sizes of 4 spaces or 1 tab
            var indent = LexerUtilities.GetNextCharactersInCollection(
                input, 
                startingIndex,
                Consts.Indentations,
                CollectionType.Inclusive
            );
        
            var totalIndent = LexerUtilities.GetIndentValueFromIndentationString(indent);
            // This is the number of characters in the indent plus the newline
            _index += indent.Length;
            if (includeNewline)
            {
                _index++;
                // We want to update these before we create the token because we want any errors to be associated to
                // the indents on the second line, not the return on the first
                _line++;
                _lineContents = GetLineContents();
            }
            _tokens.Add(new Token(Consts.TokenTypes.Newline, totalIndent.ToString(), _line, fileName, _lineContents));
        }
        
        void HandleNumber()
        {
            var numberCharacters = LexerUtilities.GetNextCharactersInCollection(
                input, 
                _index, 
                Consts.NumberCharacters, 
                CollectionType.Inclusive
            );

            _tokens.Add(new Token(Consts.TokenTypes.Numeric, numberCharacters, _line, fileName, _lineContents));
            _index += numberCharacters.Length;
        }

        void HandleString()
        {
            var startingQuoteCollection = new [] { input[_index] };
            var stringContents = LexerUtilities.GetNextCharactersInCollection(
                input,
                _index + 1,
                startingQuoteCollection,
                CollectionType.Exclusive
            );
                
            if (_index + stringContents.Length + 1 >= input.Length)
            {
                throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "EOL while scanning string literal");
            }
            
            _tokens.Add(new Token(Consts.TokenTypes.String, stringContents, _line, fileName, _lineContents));
            _index += stringContents.Length + 2;
        }

        void HandleWord()
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
            else if (Consts.ConstantStrings.Contains(word))
            {
                type = Consts.TokenTypes.Constant;
            }
            else if (Consts.Operators.Contains(word))
            {
                type = Consts.TokenTypes.Operator;
            }
            else if (Consts.BuiltInFunctions.Contains(word))
            {
                type = Consts.TokenTypes.BuiltIn;
            }
            else if (Consts.PrincipleTypes.Contains(word))
            {
                type = Consts.TokenTypes.PrincipleType;
            }
            
            _tokens.Add(new Token(type, word, _line, fileName, _lineContents));
            _index += word.Length;
        }
        
        void HandleComment()
        {
            // Get all next characters up until the newline
            var comment = LexerUtilities.GetNextCharactersInCollection(
                input, 
                _index,
                ['\n'], 
                CollectionType.Exclusive
            );
            _index += comment.Length;
        }
    }
    
}