namespace BattleScript;

using System.Collections.Generic;

public class Consts {
	public static char[] Quotes = new char[] {'\'', '\"'};
	public static char[] Whitespace = new char[] {'\t', '\n', ' '};
	public static char[] Digits = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
	public static char[] NumberCharacters = Digits.Concat(new char[] {'.'}).ToArray();
	
	public static char[] Letters = new char[] {
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 
		'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 
		'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
	};
	public static char[] StartingWordCharacters = Letters.Concat(new char[] { '_' }).ToArray();
	public static char[] WordCharacters = StartingWordCharacters.Concat(Digits).ToArray();

	public static string[] Keywords = new string[] {
		"var", "if", "else", "while", "function", "return", "import", "export", "class", "extends", 
		"Btl", "constructor", "self"
	};
	public static char[] Separators = new char[] {'(', ')', '{', '}', ',', '[', ']', ':', '.'};
	public static string[] Operators = new string[] {"==", "<", ">", "+", "*"};
	public static string[] Booleans = new string[] {"true", "false"};
	
	public static string[] OpeningSeparators = new string[] {"(", "{", "["};
	public static string[] ClosingSeparators = new string[] {")", "}", "]"};
	public static string[] MatchedSeparators = new string[] {"(", ")", "{", "}", "[", "]"};
	public static Dictionary<string, string> MatchingSeparatorsMap = new Dictionary<string, string>() {
		{"(", ")"},
		{"{", "}"},
		{"[", "]"},
		{")", "("},
		{"}", "{"},
		{"]", "["}
	};
	
	public enum OutputTypes {Error, Log};
	public enum ErrorTypes {
		InvalidToken,
		MisplacedToken,
		WrongNumberOfArgs,
		UndefinedIdentifier
	};
	public static string[] ErrorTypeStrings = new string[] {
		"InvalidToken",
		"MisplacedToken",
		"WrongNumberOfArgs",
		"UndefinedIdentifier"
	};
	
	public enum TokenTypes {
		Number,
		String,
		Boolean,
		Keyword,
		Identifier,
		Assignment,
		Separator,
		Operator,
		Semicolon,
		Whitespace,
		Comment,
		Error
	};
	public static string[] TokenTypeStrings = new string[] {
		"Number",
		"String",
		"Boolean",
		"Keyword",
		"Identifier",
		"Assignment",
		"Separator",
		"Operator",
		"Semicolon",
		"Whitespace",
		"Comment",
		"Error"
	};
	
	public enum InstructionTypes {
		Assignment,				// 0
		Declaration,			// 1
		Builtin,				// 2
		Operation,				// 3
		Variable,				// 4
		Number,					// 5
		String,					// 6
		Boolean,				// 7
		If,						// 8
		While,					// 9
		SquareBraces,			// 10
		Dictionary,				// 11
		Import,					// 12
		Function,				// 13
		Parens,					// 14
		Return,					// 15
		Class,					// 16
		Else,					// 17
		Constructor,			// 18
		Self, 					// 19
		Super					// 20
	};

	public static string[] InstructionTypeStrings = new string[] {
 		"Assignment",
		"Declaration",
		"Builtin",
		"Operation",
		"Variable",
		"Number",
		"String",
		"Boolean",
		"If",
		"While",
		"SquareBraces",
		"Dictionary",
		"Import",
		"Function",
		"Parens",
		"Return", 
		"Class",
		"Member",
		"Else",
		"Constructor",
		"Self",
		"Super"
	};

	public enum VariableTypes {
		Value,
		Array,
		Dictionary,
		Function,
		Class,
		Object
	};
}



