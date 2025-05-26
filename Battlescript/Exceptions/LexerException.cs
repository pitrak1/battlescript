namespace Battlescript;

public class LexerException(string invalidCharacters, int line, int column) : Exception(
    "Lexer Error: Invalid character found at line " + line + ", column " + column + ":" + invalidCharacters);