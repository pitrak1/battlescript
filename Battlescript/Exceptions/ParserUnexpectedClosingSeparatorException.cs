namespace Battlescript;

public class ParserUnexpectedClosingSeparatorException(Token token) : Exception(
    "Parser Error: Unexpected closing separator found at line " + token.Line + ", column " + token.Column + ":" + token.Value);