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
	public static char[] StartingIdentifierCharacters = Letters.Concat(new char[] { '_' }).ToArray();
	
	public static string[] Keywords = new string[] {
		"var", "if", "else", "while", "function", "return", "import", "export", "class", "extends", 
		"Btl", "constructor"
	};
	public static char[] Separators = new char[] {'(', ')', '{', '}', ',', '[', ']', ':', '.'};
	public static string[] Operators = new string[] {"==", "<", ">", "+", "*"};
	public static string[] Booleans = new string[] {"true", "false"};
	
	public static char[] OpeningSeparators = new char[] {'(', '{', '['};
	public static char[] ClosingSeparators = new char[] {')', '}', ']'};
	public static char[] MatchedSeparators = new char[] {'(', ')', '{', '}', '[', ']'};
	public static Dictionary<char, char> MatchingSeparatorsMap = new Dictionary<char, char>() {
		{'(', ')'},
		{'{', '}'},
		{'[', ']'},
		{')', '('},
		{'}', '{'},
		{']', '['}
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
		Member,					// 17
		Else,					// 18
		Constructor,			// 19
		Self, 					// 20
		Super					// 21
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
		Function,
		Class,
		Object
	};
}



