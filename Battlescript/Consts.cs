using System.Collections.Frozen;
using System.Collections.Immutable;

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

    public static readonly FrozenDictionary<string, string> MatchingBracketsMap = new Dictionary<string, string>
    {
        { "(", ")" },
        { "{", "}" },
        { "[", "]" },
        { ")", "(" },
        { "}", "{" },
        { "]", "[" }
    }.ToFrozenDictionary();
    
    public static readonly ImmutableArray<string> BuiltInFunctions =
    [
        "abs",
        // "aiter", // NOT SUPPORTED IN V1
        "all",
        // "anext", // NOT SUPPORTED IN V1
        "any",
        // "ascii", // NOT SUPPORTED IN V1
        // "bin", // NOT SUPPORTED IN V1
        // "breakpoint", // Need more sophisticated debugging stuff before this
        // "bytearray", // NOT SUPPORTED IN V1
        // "bytes", // NOT SUPPORTED IN V1
        "callable",
        // "chr", // NOT SUPPORTED IN V1
        // "classmethod", // Need support for *args, **kwargs first
        // "compile", // NOT SUPPORTED IN V1
        // "complex", // NOT SUPPORTED IN V1
        "delattr",
        // "dir", // May not be necessary, we'll see
        // "divmod",
        // "enumerate",
        // "eval", // NOT SUPPORTED IN V1
        // "exec", // NOT SUPPORTED IN V1
        // "filter",
        // "format",
        // "frozenset",
        // "getattr",
        // "globals",
        "hasattr",
        // "hash",
        // "help", // NOT SUPPORTED IN V1
        // "hex", // NOT SUPPORTED IN V1
        // "id",
        // "input",
        "isinstance",
        "issubclass",
        // "iter",
        "len",
        // "locals",
        // "map",
        // "max",
        // "memoryview",
        // "min",
        // "next",
        // "object", // NOT SUPPORTED IN V1
        // "oct", // NOT SUPPORTED IN V1
        // "open", // NOT SUPPORTED IN V1
        // "ord", // NOT SUPPORTED IN V1
        // "pow",
        "print",
        // "property",
        "range",
        // "repr",
        // "reversed",
        // "round",
        // "set", // Can do, but might be post v1
        // "setattr",
        // "slice",
        // "sorted",
        // "staticmethod",
        // "sum",
        // "super",
        // "tuple", // Can do, but might be post v1
        "type",
        // "vars",
        // "zip"
    ];
    
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
        Breakpoint,
        Numeric,
        FormattedString,
        Binding,
        SpecialVariable
    }
}