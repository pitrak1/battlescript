namespace BattleScript; 

public class Lexer {
    public static List<Token> Run(string contents) {
        List<Token> tokens = new List<Token>();

        string[] lines = contents.Split('\n');

        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++) {
            string line = lines[lineNumber];
            HandleLine(tokens, line, lineNumber);
        }
        
        return tokens;
    }

    private static void HandleLine(List<Token> tokens, string line, int lineNumber) {
        int lineIndex = 0;
        while (lineIndex < line.Length) {
            Token token = HandleNextToken(line, lineIndex);

            switch (token.Type) {
                case Consts.TokenTypes.Comment:
                    return;
                case Consts.TokenTypes.Whitespace:
                    lineIndex++;
                    break;
                default:
                    token.SetDebugInfo(lineNumber, lineIndex);
                    tokens.Add(token);
                    lineIndex += token.Value.Length;
                    break;
            }
        }
    }

    private static Token HandleNextToken(string line, int lineIndex) {
        string nextCharacters = line.Substring(lineIndex, 2);

        if (Consts.Whitespace.Contains(nextCharacters[0])) {
            return new Token(Consts.TokenTypes.Whitespace, "");
        // } else if (Consts.Digits.Contains(nextCharacters[0])) {
        //
        } else if (Consts.Quotes.Contains(nextCharacters[0])) {
            return HandleString(line, lineIndex);
            // } else if (Consts.Separators.Contains(nextCharacters[0])) {
            //     
            // } else if (Consts.Letters.Contains(nextCharacters[0]) || nextCharacters[0] == '_') {
            //     
            // } else if (Consts.Operators.Contains(nextCharacters)) {
            //     
            // } else if (Consts.Operators.Contains(nextCharacters[0].ToString())) {
            //     
            // } else if (nextCharacters[0] == '=') {
            //     
            // } else if (nextCharacters[0] == ';') {
            //     
            // } else if (nextCharacters == "//") {

        } else {
            throw new SystemException("Invalid character found");
        }
    }

    private static Token HandleString(string line, int lineIndex) {
        char startingQuote = line[lineIndex];
        lineIndex++;
        string result = LexerUtilities.GetLineUntilCharacters(line, lineIndex, new char[] {startingQuote});
        string finalString = startingQuote + result + startingQuote;
        return new Token(Consts.TokenTypes.String, finalString);
    }
}