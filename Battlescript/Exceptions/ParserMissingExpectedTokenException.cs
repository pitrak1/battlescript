namespace Battlescript;

public class ParserMissingExpectedTokenException(Token token, string tokenValue) : Exception(
    "Parser Error: Expected token " + tokenValue + " at line " + token.Line + ", column " + token.Column);