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
    public static readonly char[] StartingWordCharacters = Letters.Concat(new char['_']).ToArray();
    public static readonly char[] WordCharacters = StartingWordCharacters.Concat(Digits).ToArray();
    public static readonly char[] Indentations = [' ', '\t'];
    public static readonly char[] Quotes = ['\'', '"'];
    public static readonly char[] Separators = ['(', ')', '{', '}', ',', '[', ']', ':', '.'];
    public static readonly string[] Keywords = [
        "None",
        "and",
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
        "in",
        "is",
        "lambda",
        "nonlocal",
        "not",
        "or",
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
        "+",
        "-",
        "*",
        "/",
        "%",
        "**",
        "//",
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
        ":=",
        "==",
        "!=",
        ">",
        "<",
        ">=",
        "<=",
        "not",
        "and",
        "or",
        "is",
        "in",
        "&",
        "|",
        "~",
        "^",
        "<<",
        ">>"
    ];

    public enum TokenTypes
    {
        Number,
        String,
        Boolean,
        Keyword,
        Newline,
        Identifier,
        Assignment,
        Separator,
        Operator,
        Semicolon,
        Whitespace,
        Comment,
        Breakpoint
    }
}