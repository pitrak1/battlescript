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
		"Btl", "constructor", "self", "const", "super"
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
		Comment
	};

	public enum InstructionTypes {
		Assignment,
		Declaration,
		Operation,
		Variable,
		Number,
		String,
		Boolean,
		If,
		While,
		SquareBraces,
		Dictionary,
		Function,
		Parens,
		Return,
		Class,
		Else,
		Constructor,
		Self,
		Super,
		Btl,
		ConstDeclaration // Does not yet have an instruction class
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



