namespace Battlescript;

public class Lexer(string input, string? fileName = null)
{
    private int _index;
    private int _line = 1;
    private string _expressionForStacktrace = "";
    
    private readonly List<Token> _tokens = [];

    public List<Token> Run()
    {
        _expressionForStacktrace = GetLineContents();
        
        while (_index < input.Length)
        {
            var (nextNextNextCharacter, nextNextCharacter, nextCharacter) = 
                LexerUtilities.GetNextThreeCharacters(input, _index);
            
            if (nextCharacter[0] == '\n')
            {
                HandleReturn();
            }
            else if (IsLineSplitWithBackslash(nextCharacter, nextNextCharacter))
            {
                _index += 2;
            }
            else if (nextCharacter[0] == ' ')
            {
                _index++;
            } 
            else if (IsNumber(nextCharacter, nextNextCharacter))
            {
                HandleNumber();
            }
            else if (Consts.Quotes.Contains(nextCharacter[0]))
            {
                HandleString();
            }
            else if (Consts.Brackets.Contains(nextCharacter[0]))
            {
                _tokens.Add(new Token(
                    Consts.TokenTypes.Bracket, 
                    nextCharacter, 
                    _line, 
                    fileName, 
                    _expressionForStacktrace
                ));
                _index++;
            }
            else if (Consts.Delimiters.Contains(nextCharacter[0]))
            {
                _tokens.Add(new Token(
                    Consts.TokenTypes.Delimiter, 
                    nextCharacter, 
                    _line, 
                    fileName, 
                    _expressionForStacktrace
                ));
                _index++;
            }
            else if (nextCharacter[0] == '.')
            {
                _tokens.Add(new Token(
                    Consts.TokenTypes.Period, 
                    nextCharacter, 
                    _line, 
                    fileName, 
                    _expressionForStacktrace
                ));
                _index++;
            }
            else if (Consts.StartingWordCharacters.Contains(nextCharacter[0]))
            {
                HandleWord();
            }
            else if (IsOperatorOrAssignment(nextCharacter, nextNextCharacter, nextNextNextCharacter))
            {
                HandleOperatorOrAssignment(nextCharacter, nextNextCharacter, nextNextNextCharacter);
            }
            else if (nextCharacter[0] == '#')
            {
                HandleComment();
            }
            else if (nextCharacter[0] == '\t')
            {
                throw new InternalRaiseException(BsTypes.Types.SyntaxError, "unexpected indent");
            }
            else
            {
                throw new InternalRaiseException(BsTypes.Types.SyntaxError, "invalid syntax");
            }
        }
        
        return _tokens;
        
        bool IsLineSplitWithBackslash(string nextCharacter, string nextNextCharacter)
        {
            return nextCharacter[0] == '\\' && nextNextCharacter[0] == '\n';
        }

        bool IsNumber(string nextCharacter, string nextNextCharacter)
        {
            if (nextCharacter[0] == '.' && nextNextCharacter == "")
            {
                return false;
            }

            var isNumberStartingWithDecimalPoint = nextCharacter[0] == '.' && Consts.Digits.Contains(nextNextCharacter[0]);
            var isNumberStartingWithDigit = Consts.Digits.Contains(nextCharacter[0]);
            return isNumberStartingWithDecimalPoint || isNumberStartingWithDigit;
        }
        
        bool IsOperatorOrAssignment(string nextCharacter, string nextNextCharacter, string nextNextNextCharacter)
        {
            return Consts.Operators.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter) || 
                   Consts.Assignments.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter) ||
                   Consts.Operators.Contains(nextCharacter + nextNextCharacter) ||
                   Consts.Assignments.Contains(nextCharacter + nextNextCharacter) ||
                   Consts.Operators.Contains(nextCharacter) ||
                   Consts.Assignments.Contains(nextCharacter);
        }
    }
    
    private string GetLineContents()
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
    
    private void HandleReturn()
    {
        var startingIndex = _index + 1;
        
        // We are assuming indent sizes of 4 spaces or 1 tab
        var indentString = LexerUtilities.GetNextCharactersInCollection(
            input, 
            startingIndex,
            Consts.Indentations,
            CollectionType.Inclusive
        );
    
        // This is <# of tabs> + floor(<# of spaces>/4)
        var indentValue = LexerUtilities.GetIndentValueFromIndentationString(indentString);
        
        _index += indentString.Length + 1;
        // We want to update these before we create the token because we want any errors to be associated to
        // the indents on the second line, not the return on the first
        _line++;
        _expressionForStacktrace = GetLineContents();
        
        _tokens.Add(new Token(
            Consts.TokenTypes.Newline, 
            indentValue.ToString(), 
            _line, 
            fileName, 
            _expressionForStacktrace
        ));
    }
    
    private void HandleNumber()
    {
        var numberCharacters = LexerUtilities.GetNextCharactersInCollection(
            input, 
            _index, 
            Consts.NumberCharacters, 
            CollectionType.Inclusive
        );

        _tokens.Add(new Token(
            Consts.TokenTypes.Numeric, 
            numberCharacters, 
            _line, 
            fileName, 
            _expressionForStacktrace
        ));
        _index += numberCharacters.Length;
    }

    private void HandleString()
    {
        var startingQuoteCollection = new [] { input[_index] };
        var (stringLength, stringContents) = LexerUtilities.GetNextCharactersInCollectionIncludingEscapes(
            input,
            _index + 1,
            startingQuoteCollection,
            CollectionType.Exclusive
        );
            
        if (_index + stringLength + 1 >= input.Length)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "EOL while scanning string literal");
        }
        
        _tokens.Add(new Token(Consts.TokenTypes.String, stringContents, _line, fileName, _expressionForStacktrace));
        _index += stringLength + 2;
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
        else if (Consts.ConversionTypes.Contains(word))
        {
            type = Consts.TokenTypes.ConversionType;
        }
        
        _tokens.Add(new Token(type, word, _line, fileName, _expressionForStacktrace));
        _index += word.Length;
    }

    private void HandleOperatorOrAssignment(string nextCharacter, string nextNextCharacter, string nextNextNextCharacter)
    {
        if (Consts.Operators.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter))
        {
            _tokens.Add(new Token(
                Consts.TokenTypes.Operator,
                nextCharacter + nextNextCharacter + nextNextNextCharacter,
                _line,
                fileName,
                _expressionForStacktrace
            ));
            _index += 3;
        }
        else if (Consts.Assignments.Contains(nextCharacter + nextNextCharacter + nextNextNextCharacter))
        {
            _tokens.Add(new Token(
                Consts.TokenTypes.Assignment,
                nextCharacter + nextNextCharacter + nextNextNextCharacter,
                _line,
                fileName,
                _expressionForStacktrace
            ));
            _index += 3;
        }
        else if (Consts.Operators.Contains(nextCharacter + nextNextCharacter))
        {
            _tokens.Add(new Token(
                Consts.TokenTypes.Operator,
                nextCharacter + nextNextCharacter,
                _line,
                fileName,
                _expressionForStacktrace
            ));
            _index += 2;
        }
        else if (Consts.Assignments.Contains(nextCharacter + nextNextCharacter))
        {
            _tokens.Add(new Token(
                Consts.TokenTypes.Assignment,
                nextCharacter + nextNextCharacter,
                _line,
                fileName,
                _expressionForStacktrace
            ));
            _index += 2;
        }
        else if (Consts.Operators.Contains(nextCharacter))
        {
            _tokens.Add(new Token(
                Consts.TokenTypes.Operator,
                nextCharacter,
                _line,
                fileName,
                _expressionForStacktrace
            ));
            _index++;
        }
        else if (Consts.Assignments.Contains(nextCharacter))
        {
            _tokens.Add(new Token(
                Consts.TokenTypes.Assignment,
                nextCharacter,
                _line,
                fileName,
                _expressionForStacktrace
            ));
            _index++;
        }
    }
    
    private void HandleComment()
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