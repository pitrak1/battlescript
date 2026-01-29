using System.Collections.Frozen;
using System.Diagnostics;

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
            var nextCharacters = GetLookaheadCharacters(input, _index);

            if (TryHandleNewline(nextCharacters)) continue;
            if (TryHandleLineContinuation(nextCharacters)) continue;
            if (TryHandleWhitespace(nextCharacters)) continue;
            if (TryHandleNumber(nextCharacters)) continue;
            if (TryHandleString(nextCharacters)) continue;
            if (TryHandleBracket(nextCharacters)) continue;
            if (TryHandleDelimiter(nextCharacters)) continue;
            if (TryHandlePeriod(nextCharacters)) continue;
            if (TryHandleWord(nextCharacters)) continue;
            if (TryHandleOperatorOrAssignment(nextCharacters)) continue;
            if (TryHandleComment(nextCharacters)) continue;

            HandleUnexpectedCharacter(nextCharacters);
        }

        return _tokens;
    }

    private bool TryHandleNewline(string nextCharacters)
    {
        if (nextCharacters[0] != '\n') return false;

        var (indentLength, indentValue) = LexerUtilities.GetIndentValue(input, _index + 1);

        _index += indentLength + 1;
        // We want to update these before we create the token because we want any errors to be associated to
        // the indents on the second line, not the return on the first
        _line++;
        _expressionForStacktrace = GetLineContents();

        AddTokenAndMoveIndex(Consts.TokenTypes.Newline, indentValue, 0);
        return true;
    }

    private bool TryHandleLineContinuation(string nextCharacters)
    {
        bool isBackslashFollowedByReturn = nextCharacters.Length >= 2 && nextCharacters[..2] == "\\\n";
        if (!isBackslashFollowedByReturn) return false;

        _index += 2;
        return true;
    }

    private bool TryHandleWhitespace(string nextCharacters)
    {
        if (nextCharacters[0] != ' ') return false;

        _index++;
        return true;
    }

    private bool TryHandleNumber(string nextCharacters)
    {
        var isNumberStartingWithDecimalPoint = nextCharacters.Length > 1 && nextCharacters[0] == '.' && Consts.IsDigit(nextCharacters[1]);
        var isNumberStartingWithDigit = nextCharacters.Length > 0 && Consts.IsDigit(nextCharacters[0]);
        var isNumber = isNumberStartingWithDecimalPoint || isNumberStartingWithDigit;

        if (!isNumber) return false;

        var numberCharacters = LexerUtilities.GetNextCharactersWhile(
            input,
            _index,
            Consts.IsNumberChar
        );
        AddTokenAndMoveIndex(Consts.TokenTypes.Numeric, numberCharacters.Result);
        return true;
    }

    private bool TryHandleString(string nextCharacters)
    {
        if (!Consts.IsQuote(nextCharacters[0])) return false;

        var (stringLength, stringContents) = LexerUtilities.GetStringWithEscapes(input, _index);
        const int quoteCharacterCount = 2;
        AddTokenAndMoveIndex(Consts.TokenTypes.String, stringContents, stringLength + quoteCharacterCount);
        return true;
    }

    private bool TryHandleBracket(string nextCharacters)
    {
        if (!Consts.IsBracket(nextCharacters[0])) return false;

        AddTokenAndMoveIndex(Consts.TokenTypes.Bracket, nextCharacters[0].ToString());
        return true;
    }

    private bool TryHandleDelimiter(string nextCharacters)
    {
        if (!Consts.IsDelimiter(nextCharacters[0])) return false;

        AddTokenAndMoveIndex(Consts.TokenTypes.Delimiter, nextCharacters[0].ToString());
        return true;
    }

    private bool TryHandlePeriod(string nextCharacters)
    {
        if (nextCharacters[0] != '.') return false;

        AddTokenAndMoveIndex(Consts.TokenTypes.Period, nextCharacters[0].ToString());
        return true;
    }

    private bool TryHandleWord(string nextCharacters)
    {
        Consts.TokenTypes GetTokenTypeFromWord(string word)
        {
            if (Keywords.Contains(word))
                return Consts.TokenTypes.Keyword;
            if (ConstantStrings.Contains(word))
                return Consts.TokenTypes.Constant;
            if (Operators.Contains(word))
                return Consts.TokenTypes.Operator;
            if (BuiltInFunctions.Contains(word))
                return Consts.TokenTypes.BuiltIn;
            if (Bindings.Contains(word))
                return Consts.TokenTypes.Binding;
            return Consts.TokenTypes.Identifier;
        }

        if (!Consts.IsWordStartChar(nextCharacters[0])) return false;

        var word = LexerUtilities.GetNextCharactersWhile(
            input,
            _index,
            Consts.IsWordChar
        );
        var type = GetTokenTypeFromWord(word.Result);
        AddTokenAndMoveIndex(type, word.Result);
        return true;
    }

    private bool TryHandleOperatorOrAssignment(string nextCharacters)
    {
        string? GetMatchingOperatorOrAssignment()
        {
            if (nextCharacters.Length == 3)
            {
                if (Operators.Contains(nextCharacters) || Assignments.Contains(nextCharacters))
                    return nextCharacters;
            }

            if (nextCharacters.Length >= 2)
            {
                var firstTwoCharacters = nextCharacters[..2];
                if (Operators.Contains(firstTwoCharacters) || Assignments.Contains(firstTwoCharacters))
                    return firstTwoCharacters;
            }

            if (nextCharacters.Length >= 1)
            {
                var firstCharacterOnly = nextCharacters[0].ToString();
                if (Operators.Contains(firstCharacterOnly) || Assignments.Contains(firstCharacterOnly))
                    return firstCharacterOnly;
            }

            return null;
        }

        if (GetMatchingOperatorOrAssignment() is not { } operatorOrAssignmentValue) return false;

        var tokenType = Operators.Contains(operatorOrAssignmentValue)
            ? Consts.TokenTypes.Operator
            : Consts.TokenTypes.Assignment;
        AddTokenAndMoveIndex(tokenType, operatorOrAssignmentValue);
        return true;
    }

    private bool TryHandleComment(string nextCharacters)
    {
        if (nextCharacters[0] != '#') return false;

        var charactersUntilNewline = LexerUtilities.GetNextCharactersWhile(
            input,
            _index,
            c => c != '\n'
        );
        _index += charactersUntilNewline.Length;
        return true;
    }

    private static void HandleUnexpectedCharacter(string nextCharacters)
    {
        if (nextCharacters[0] == '\t')
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "unexpected indent");
        }

        throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "invalid syntax");
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
    
    private string GetLineContents()
    {
        var newlineIndex = input.IndexOf('\n', _index);
        return newlineIndex != -1
            ? input[_index..newlineIndex]
            : input[_index..];
    }
    
    private string GetLookaheadCharacters(string inputString, int index)
    {
        const int maxLengthOfOperators = 3;
        return inputString.Substring(index, Math.Min(inputString.Length - index, maxLengthOfOperators));
    }
    
    #region Constants
    
    private static readonly FrozenSet<string> Operators = FrozenSet.ToFrozenSet([
        "**",
        "~", // NOT SUPPORTED IN V1
        "*",
        "/",
        "//",
        "%",
        "+",
        "-",
        "<<", // NOT SUPPORTED IN V1
        ">>", // NOT SUPPORTED IN V1
        "&", // NOT SUPPORTED IN V1
        "^", // NOT SUPPORTED IN V1
        "|", // NOT SUPPORTED IN V1
        "==",
        "!=",
        ">",
        ">=",
        "<",
        "<=",
        "is",
        "is not",
        "in",
        "not in",
        "not",
        "and",
        "or"
    ]);
    
    private static readonly FrozenSet<string> Assignments = FrozenSet.ToFrozenSet([
        "=",
        "+=",
        "-=",
        "*=",
        "/=",
        "%=",
        "//=",
        "**=",
        "&=", // NOT SUPPORTED IN V1
        "|=", // NOT SUPPORTED IN V1
        "^=", // NOT SUPPORTED IN V1
        ">>=", // NOT SUPPORTED IN V1
        "<<=", // NOT SUPPORTED IN V1
        ":=" // NOT SUPPORTED IN V1
    ]);
    
    private static readonly FrozenSet<string> Keywords = FrozenSet.ToFrozenSet([
        "as",
        "assert",
        "async", // NOT SUPPORTED IN V1
        "await", // NOT SUPPORTED IN V1
        "break",
        "class",
        "continue",
        "def",
        "del",
        "elif",
        "else",
        "except",
        "finally",
        "for",
        "from",
        "global",
        "if",
        "import",
        "lambda",
        "match", // NOT SUPPORTED IN V1
        "nonlocal",
        "pass",
        "raise",
        "return",
        "try",
        "while",
        "with", // NOT SUPPORTED IN V1
        "yield" // NOT SUPPORTED IN V1
    ]);

    private static readonly FrozenSet<string> BuiltInFunctions = FrozenSet.ToFrozenSet([
        "aiter", // NOT SUPPORTED IN V1
        "all",
        "anext", // NOT SUPPORTED IN V1
        "any",
        "ascii",
        "bin", // NOT SUPPORTED IN V1
        "breakpoint",
        "bytearray", // NOT SUPPORTED IN V1
        "bytes", // NOT SUPPORTED IN V1
        "callable",
        "chr", // NOT SUPPORTED IN V1
        "classmethod",
        "compile", // NOT SUPPORTED IN V1
        "complex", // NOT SUPPORTED IN V1
        "delattr",
        "dir",
        "divmod",
        "enumerate",
        "eval", // NOT SUPPORTED IN V1
        "exec", // NOT SUPPORTED IN V1
        "filter",
        "format",
        "frozenset",
        "getattr",
        "globals",
        "hash",
        "help", // NOT SUPPORTED IN V1
        "hex", // NOT SUPPORTED IN V1
        "id",
        "input",
        "iter",
        "len",
        "locals",
        "map",
        "max",
        "memoryview",
        "min",
        "next",
        "oct", // NOT SUPPORTED IN V1
        "open", // NOT SUPPORTED IN V1
        "ord", // NOT SUPPORTED IN V1
        "pow",
        "print",
        "property",
        "repr",
        "reversed",
        "round",
        "set", // Can do, but might be post v1
        "setattr",
        "slice",
        "sorted",
        "staticmethod",
        "sum",
        "super",
        "tuple", // Can do, but might be post v1
        "type",
        "vars",
        "zip"
    ]);
    
    private static readonly FrozenSet<string> ConstantStrings =
        FrozenSet.ToFrozenSet(["None", "True", "False"]);
    
    private static readonly string[] Bindings =
    [
        "__btl_numeric__",
        "__btl_sequence__",
        "__btl_mapping__",
        "__btl_string__",
        "__btl_getattr__",
        "__btl_isinstance__",
        "__btl_issubclass__",
    ];
    
    #endregion
}