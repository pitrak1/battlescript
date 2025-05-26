namespace Battlescript;

public class ParserUnexpectedTokenException(Token token) : Exception(
    "Parser Error: Unexpected token found at line " + token.Line + ", column " + token.Column + ":" + token.Value);