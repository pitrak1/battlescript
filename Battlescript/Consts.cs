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

    public static readonly FrozenDictionary<string, string> MatchingBracketsMap = new Dictionary<string, string>
    {
        { "(", ")" },
        { "{", "}" },
        { "[", "]" },
        { ")", "(" },
        { "}", "{" },
        { "]", "[" }
    }.ToFrozenDictionary();
    
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
}