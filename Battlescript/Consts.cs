namespace Battlescript;

public static class Consts
{
    public static readonly char[] Digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    public static readonly char[] Letters = [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    ];
    public static readonly char[] StartingWordCharacters = [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '_'
    ];
    public static readonly char[] WordCharacters = [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '_',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    ];
    public static readonly char[] NumberCharacters = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'];
    public static readonly char[] Indentations = [' ', '\t'];
    public static readonly char[] Quotes = ['\'', '"'];
    public static readonly char[] Separators = ['(', ')', '{', '}', ',', '[', ']', ':', '.'];
    public static readonly string[] OpeningSeparators = ["(", "{", "["];
    public static readonly string[] ClosingSeparators = [")", "}", "]"];
    public static readonly Dictionary<string, string> MatchingSeparatorsMap = new() {
        {"(", ")"},
        {"{", "}"},
        {"[", "]"},
        {")", "("},
        {"}", "{"},
        {"]", "["}
    };
    public static readonly string[] Keywords = [
        "None",
        "as",
        "assert",
        "async",
        "await",
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
        "nonlocal",
        "pass",
        "raise",
        "return",
        "try",
        "while",
        "with",
        "yield"
    ];
    public static readonly string[] Booleans = ["True", "False"];

    public static readonly string[] Operators = [
        "**",
        "~",
        "*",
        "/",
        "//",
        "%",
        "+",
        "-",
        "<<",
        ">>",
        "&",
        "^",
        "|",
        "==",
        "!=",
        ">",
        ">=",
        "<",
        "<=",
        "is",
        "in",
        "not",
        "and",
        "or"
    ];

    public static readonly string[] Assignments = [
        "=",
        "+=",
        "-=",
        "*=",
        "/=",
        "%=",
        "//=",
        "**=",
        "&=",
        "|=",
        "^=",
        ">>=",
        "<<=",
        ":="
    ];

    public enum TokenTypes
    {
        Number,
        String,
        Boolean,
        Keyword,
        Newline,
        Identifier,
        Separator,
        Operator,
        Assignment,
        Breakpoint
    }
    
    public enum InstructionTypes
    {
        Assignment,
        Operation,
        Variable,
        Number,
        String,
        Boolean,
        If,
        For,
        While,
        SquareBrackets,
        SetDefinition,
        DictionaryDefinition,
        Function,
        Parens,
        Return,
        Else,
        Btl,
        Breakpoint
    };
    
    public enum VariableTypes
    {
        Null,
        Number,
        String,
        Boolean,
        List,
        Dictionary,
        Set,
        Tuple,
        Function
    };
}