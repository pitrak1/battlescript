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
            var nextCharacters = GetNextThreeCharactersOrUntilEndOfString(input, _index);
            
            if (nextCharacters[0] == '\n')
            {
                var (indentLength, indentValue) = LexerUtilities.GetIndentValue(input, _index + 1);
                
                _index += indentLength + 1;
                // We want to update these before we create the token because we want any errors to be associated to
                // the indents on the second line, not the return on the first
                _line++;
                _expressionForStacktrace = GetLineContents();
                
                AddTokenAndMoveIndex(Consts.TokenTypes.Newline, indentValue, 0);
            }
            else if (IsBackslashFollowedByReturn(nextCharacters))
            {
                _index += 2;
            }
            else if (nextCharacters[0] == ' ')
            {
                _index++;
            } 
            else if (IsNumber(nextCharacters))
            {
                var numberCharacters = LexerUtilities.GetNextCharactersWhile(
                    input,
                    _index,
                    Consts.IsNumberChar
                );
                AddTokenAndMoveIndex(Consts.TokenTypes.Numeric, numberCharacters.Result);
            }
            else if (Consts.IsQuote(nextCharacters[0]))
            {
                var (stringLength, stringContents) = LexerUtilities.GetStringWithEscapes(input, _index);
                AddTokenAndMoveIndex(Consts.TokenTypes.String, stringContents, stringLength + 2);
            }
            else if (Consts.IsBracket(nextCharacters[0]))
            {
                AddTokenAndMoveIndex(Consts.TokenTypes.Bracket, nextCharacters[0].ToString());
            }
            else if (Consts.IsDelimiter(nextCharacters[0]))
            {
                AddTokenAndMoveIndex(Consts.TokenTypes.Delimiter, nextCharacters[0].ToString());
            }
            else if (nextCharacters[0] == '.')
            {
                AddTokenAndMoveIndex(Consts.TokenTypes.Period, nextCharacters[0].ToString());
            }
            else if (Consts.IsWordStartChar(nextCharacters[0]))
            {
                var word = LexerUtilities.GetNextCharactersWhile(
                    input,
                    _index,
                    Consts.IsWordChar
                );
                var type = GetTokenTypeFromWord(word.Result);
                AddTokenAndMoveIndex(type, word.Result);
            }
            else if (GetMatchingOperatorOrAssignment(nextCharacters) is { } operatorOrAssignmentValue)
            {
                var tokenType = Consts.Operators.Contains(operatorOrAssignmentValue) ? Consts.TokenTypes.Operator : Consts.TokenTypes.Assignment;
                AddTokenAndMoveIndex(tokenType, operatorOrAssignmentValue);
            }
            else if (nextCharacters[0] == '#')
            {
                var charactersUntilNewline = LexerUtilities.GetNextCharactersWhile(
                    input,
                    _index,
                    c => c != '\n'
                );
                _index += charactersUntilNewline.Length;
            }
            else if (nextCharacters[0] == '\t')
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "unexpected indent");
            }
            else
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
            }
        }
        
        return _tokens;
    }
    
    private void AddTokenAndMoveIndex(Consts.TokenTypes type, string value, int? length = null)
    {
        _tokens.Add(new Token(
            type,
            value, 
            _line, 
            fileName, 
            _expressionForStacktrace
        ));
        _index += length ?? value.Length;
    }
    
    private string? GetMatchingOperatorOrAssignment(string nextCharacters)
    {
        if (nextCharacters.Length == 3)
        {
            if (Consts.Operators.Contains(nextCharacters) || Consts.Assignments.Contains(nextCharacters))
            {
                return nextCharacters;
            }
        }
        
        if (nextCharacters.Length >= 2)
        {
            var firstTwoCharacters = nextCharacters[..2];
            if (Consts.Operators.Contains(firstTwoCharacters) || Consts.Assignments.Contains(firstTwoCharacters))
            {
                return firstTwoCharacters;
            }
        }
        
        if (nextCharacters.Length >= 1)
        {
            var firstCharacterOnly = nextCharacters[0].ToString();
            if (Consts.Operators.Contains(firstCharacterOnly) || Consts.Assignments.Contains(firstCharacterOnly))
            {
                return firstCharacterOnly;
            }
        }

        return null;
    }
    
    private bool IsBackslashFollowedByReturn(string nextCharacters) =>
        nextCharacters.Length >= 2 && nextCharacters[..2] == "\\\n";
    
    private bool IsNumber(string nextCharacters)
    {
        var isNumberStartingWithDecimalPoint = nextCharacters.Length > 1 && nextCharacters[0] == '.' && Consts.IsDigit(nextCharacters[1]);
        var isNumberStartingWithDigit = nextCharacters.Length > 0 && Consts.IsDigit(nextCharacters[0]);
        return isNumberStartingWithDecimalPoint || isNumberStartingWithDigit;
    }
    
    private string GetLineContents()
    {
        if (input.IndexOf('\n', _index) != -1)
        {
            return input[_index..input.IndexOf('\n', _index)];
        }
        else
        {
            return input[_index..];
        }
    }
    
    private Consts.TokenTypes GetTokenTypeFromWord(string word)
    {
        if (Consts.Keywords.Contains(word))
        {
            return Consts.TokenTypes.Keyword;
        }
        else if (Consts.ConstantStrings.Contains(word))
        {
            return Consts.TokenTypes.Constant;
        }
        else if (Consts.Operators.Contains(word))
        {
            return Consts.TokenTypes.Operator;
        }
        else if (Consts.BuiltInFunctions.Contains(word))
        {
            return Consts.TokenTypes.BuiltIn;
        }
        else if (Consts.ConversionTypes.Contains(word))
        {
            return Consts.TokenTypes.ConversionType;
        }
        else
        {
            return Consts.TokenTypes.Identifier;
        }
    }
    
    private string GetNextThreeCharactersOrUntilEndOfString(string inputString, int index)
    {
        return inputString.Substring(index, Math.Min(inputString.Length - index, 3));
    }
}