using System.Collections.Frozen;

namespace Battlescript;

public static class Consts
{
    public const double FloatingPointTolerance = 0.000001;

    public static bool IsDigit(char c) => char.IsAsciiDigit(c);
    public static bool IsNumberChar(char c) => char.IsAsciiDigit(c) || c == '.';
    public static bool IsWordStartChar(char c) => char.IsAsciiLetter(c) || c == '_';
    public static bool IsWordChar(char c) => char.IsAsciiLetterOrDigit(c) || c == '_';
    public static bool IsIndentation(char c) => c is ' ' or '\t';
    public static bool IsQuote(char c) => c is '\'' or '"';
    public static bool IsDelimiter(char c) => c is ',' or ':';
    public static bool IsBracket(char c) => c is '(' or ')' or '{' or '}' or '[' or ']';

    public static readonly FrozenSet<string> OpeningBrackets = FrozenSet.ToFrozenSet(["(", "{", "["]);
    public static readonly FrozenSet<string> ClosingBrackets = FrozenSet.ToFrozenSet([")", "}", "]"]);
    
    public const string SquareBrackets = "[";
    public const string CurlyBraces = "{";
    public const string Parentheses = "(";
    public const string Period = ".";
    public const string Comma = ",";
    public const string Colon = ":";
    public const string Wildcard = "*";

    public static readonly FrozenDictionary<string, string> MatchingBracketsMap = new Dictionary<string, string>
    {
        { "(", ")" },
        { "{", "}" },
        { "[", "]" },
        { ")", "(" },
        { "}", "{" },
        { "]", "[" }
    }.ToFrozenDictionary();

    public static readonly FrozenSet<string> Keywords = FrozenSet.ToFrozenSet([
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
        "yield" // NOT SUPPORTED IN V1
    ]);

    public static readonly FrozenSet<string> BuiltInFunctions = FrozenSet.ToFrozenSet([
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
    ]);

    public static readonly FrozenSet<string> Operators = FrozenSet.ToFrozenSet([
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

    public static readonly FrozenDictionary<string, int> OperatorPriority = new Dictionary<string, int>
    {
        { "or", 0 },
        { "and", 1 },
        { "not", 2 },
        { "not in", 3 },
        { "in", 4 },
        { "is not", 5 },
        { "is", 6 },
        { "<=", 7 },
        { "<", 8 },
        { ">=", 9 },
        { ">", 10 },
        { "!=", 11 },
        { "==", 12 },
        { "-", 13 },
        { "+", 14 },
        { "%", 15 },
        { "//", 16 },
        { "/", 17 },
        { "*", 18 },
        { "-1", 19 },
        { "+1", 20 },
        { "**", 21 }
    }.ToFrozenDictionary();

    public static readonly FrozenSet<string> BooleanOperators =
        FrozenSet.ToFrozenSet(["and", "or", "not", "is", "is not", "in", "not in"]);

    public static readonly FrozenSet<string> Assignments = FrozenSet.ToFrozenSet([
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

    public static readonly FrozenSet<string> ConstantStrings =
        FrozenSet.ToFrozenSet(["None", "True", "False"]);
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
        Bracket,
        Delimiter,
        Period,
        Operator,
        Assignment,
        BuiltIn,
        Breakpoint,
        ConversionType,
        Numeric,
        FormattedString
    }
    
    public static readonly string[] ConversionTypes =
    [
        "__btl_numeric__",
        "__btl_sequence__",
        "__btl_mapping__",
        "__btl_string__",
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