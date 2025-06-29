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
        "__list__",
        "__dict__"
    ];

    // These are currently not used or implemented in any way, but I like having a list
    public static readonly string[] BuiltInFunctions =
    [
        "abs",
        "aiter",
        "all",
        "anext",
        "any",
        "ascii",
        "bin",
        "bool",
        "breakpoint",
        "bytearray",
        "bytes",
        "callable",
        "chr",
        "classmethod",
        "compile",
        "complex",
        "delattr",
        "dict",
        "dir",
        "divmod",
        "enumerate",
        "eval",
        "exec",
        "filter",
        "float",
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
        "int",
        "isinstance",
        "issubclass",
        "iter",
        "len",
        "list",
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
        "str",
        "sum",
        "super",
        "tuple",
        "type",
        "vars",
        "zip"
    ];

    public static readonly string[] OverrideMethods =
    [
        "__add__",
        "__sub__",
        "__mul__",
        "__truediv__",
        "__floordiv__",
        "__mod__",
        "__pow__",
        "__matmul__", //NOT SUPPORTED IN V1
        "__neg__",
        "__pos__",
        "__invert__", //NOT SUPPORTED IN V1
        "__eq__",
        "__ne__",
        "__lt__",
        "__gt__",
        "__le__",
        "__ge__",
        "__and__", //NOT SUPPORTED IN V1
        "__or__", //NOT SUPPORTED IN V1
        "__xor__", //NOT SUPPORTED IN V1
        "__lshift__", //NOT SUPPORTED IN V1
        "__rshift__", //NOT SUPPORTED IN V1
        "__iadd__",
        "__isub__",
        "__imul__",
        "__itruediv__",
        "__ifloordiv__",
        "__imod__",
        "__ipow__",
        "__iand__", //NOT SUPPORTED IN V1
        "__ior__", //NOT SUPPORTED IN V1
        "__ixor__", //NOT SUPPORTED IN V1
        "__ilshift__", //NOT SUPPORTED IN V1
        "__irshift__", //NOT SUPPORTED IN V1
        "__str__",
        "__repr__",
        "__len__",
        "__getitem__",
        "__setitem__"
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
        True,
        False,
    }
    
    public static readonly string[] ListMethods =
    [
        "append",
        "extend",
        "insert",
        "remove",
        "pop",
        "clear",
        "index", // I'm not sure how to handle index yet
        "count",
        "sort", // This is going to be  more involved too
        "reverse",
        "copy"
    ];
    
    public enum TokenTypes
    {
        Integer,
        Float,
        String,
        Constant,
        Keyword,
        Newline,
        Identifier,
        Separator,
        Operator,
        Assignment,
        BuiltIn,
        Breakpoint
    }

    public enum VariableTypes
    {
        Value,
        Reference
    }
}