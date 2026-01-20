namespace Battlescript;

public class InterpreterUnknownPositionalArgumentException(int count) : Exception(
    $"Too many positional arguments: {count} extra");
