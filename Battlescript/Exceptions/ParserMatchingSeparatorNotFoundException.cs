namespace Battlescript;

public class ParserMatchingSeparatorNotFoundException(Token token) : Exception(
    "Parser Error: No matching separator found for " + token.Value + " at line " + token.Line + ", column " + token.Column);