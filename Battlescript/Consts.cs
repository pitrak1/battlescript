namespace Battlescript;

public static class Consts
{
    public static readonly double FloatingPointTolerance = 0.000001;
    public static readonly char[] NumberCharacters = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'];
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
    public static readonly char[] Indentations = [' ', '\t'];
    public static readonly char[] Quotes = ['\'', '"'];
    public static readonly char[] Separators = ['(', ')', '{', '}', ',', '[', ']', ':', '.'];
    public static readonly string[] OpeningSeparators = ["(", "{", "["];
    public static readonly string[] ClosingSeparators = [")", "}", "]"];
    
    public const string SquareBrackets = "[";
    public const string CurlyBraces = "{";
    public const string Parentheses = "(";
    public const string Period = ".";
    public const string Comma = ",";
    public const string Colon = ":";

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
        "match",
        "nonlocal",
        "pass",
        "raise",
        "return",
        "try",
        "while",
        "with",
        "yield",
    ];

    public static readonly string[] BuiltInFunctions =
    [
        "abs",
        "aiter",
        "all",
        "anext",
        "any",
        "ascii",
        "bin",
        "breakpoint",
        "bytearray",
        "bytes",
        "callable",
        "chr",
        "classmethod",
        "compile",
        "complex",
        "delattr",
        "dir",
        "divmod",
        "enumerate",
        "eval",
        "exec",
        "filter",
        "format",
        "frozenset",
        "getattr",
        "globals",
        "hasattr",
        "hash",
        "help",
        "hex",
        "id",
        "input",
        "isinstance",
        "issubclass",
        "iter",
        "len",
        "locals",
        "map",
        "max",
        "memoryview",
        "min",
        "next",
        "object",
        "oct",
        "open",
        "ord",
        "pow",
        "print",
        "property",
        "range",
        "repr",
        "reversed",
        "round",
        "set",
        "setattr",
        "slice",
        "sorted",
        "staticmethod",
        "sum",
        "super",
        "tuple",
        "type",
        "vars",
        "zip"
    ];

    public static readonly string[] Operators = [
        "**",
        "~",
        "*",
        "/",
        "//",
        "%",
        "+",
        "-",
        "<<", //NOT SUPPORTED IN V1
        ">>", //NOT SUPPORTED IN V1
        "&", //NOT SUPPORTED IN V1
        "^", //NOT SUPPORTED IN V1
        "|", //NOT SUPPORTED IN V1
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
    ];
    
    public static readonly string[] CommonOperators = ["and", "or", "not", "is", "is not", "in", "not in"];
    
    public static readonly string[] NumericalOperators = [
        "**",
        "*",
        "/",
        "//",
        "%",
        "+",
        "-",
        "==",
        "!=",
        ">",
        ">=",
        "<",
        "<=",
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
        "&=",//NOT SUPPORTED IN V1
        "|=",//NOT SUPPORTED IN V1
        "^=",//NOT SUPPORTED IN V1
        ">>=",//NOT SUPPORTED IN V1
        "<<=",//NOT SUPPORTED IN V1
        ":=" //NOT SUPPORTED IN V1
    ];
    
    public static readonly string[] ConstantStrings = ["None", "True", "False"];
    public enum Constants{
        None,
    }
    
    public enum TokenTypes
    {
        String,
        Constant,
        Keyword,
        Newline,
        Identifier,
        Separator,
        Operator,
        Assignment,
        BuiltIn,
        Breakpoint,
        PrincipleType,
        Numeric,
    }
    
    public static readonly string[] PrincipleTypes =
    [
        "__numeric__",
        "__sequence__"
    ];
}