namespace Battlescript;

public class InterpreterKeywordArgBeforePositionalArgException() : Exception(
    "SyntaxError: positional argument follows keyword argument");