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
    public const string Wildcard = "*";

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
        "match",
        "nonlocal",
        "pass",
        "raise",
        "return",
        "try",
        "while",
        "with",
        "yield", // NOT SUPPORTED IN V1
    ];

    public static readonly string[] BuiltInFunctions =
    [
        "abs",
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
        "hasattr",
        "hash",
        "help", // NOT SUPPORTED IN V1
        "hex", // NOT SUPPORTED IN V1
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
        "object", // NOT SUPPORTED IN V1
        "oct", // NOT SUPPORTED IN V1
        "open", // NOT SUPPORTED IN V1
        "ord", // NOT SUPPORTED IN V1
        "pow",
        "print",
        "property",
        "range",
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
    ];

    public static readonly string[] Operators = [
        "**",
        "~", // NOT SUPPORTED IN V1
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
    
    public static readonly string[] OperatorPriority = [
        "**",
        "+1",
        "-1",
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
        "is",
        "is not",
        "in",
        "not in",
        "not",
        "and",
        "or"
    ];
    
    public static readonly string[] BooleanOperators = ["and", "or", "not", "is", "is not", "in", "not in"];
    
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
        FormattedString
    }
    
    public static readonly string[] PrincipleTypes =
    [
        "__numeric__",
        "__sequence__",
        "__mapping__",
        "__string__",
    ];
    
    public static readonly string[] InstructionTypesExpectingIndent =
    [
        "FunctionInstruction",
        "ClassInstruction",
        "ElseInstruction",
        "ExceptInstruction",
        "FinallyInstruction",
        "ForInstruction",
        "TryInstruction",
        "WhileInstruction",
        "IfInstruction",
    ];
}