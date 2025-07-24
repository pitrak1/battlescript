namespace Battlescript;

public class LexerException(string invalidCharacters, int line, string fileName) : Exception(
    $"Lexer Error: Invalid character found at file {fileName}, line {line}: {invalidCharacters}");