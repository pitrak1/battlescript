namespace Battlescript;

public class InterpreterUnknownKeywordArgumentException(IEnumerable<string> names) : Exception(
    $"Unknown keyword arguments: {string.Join(", ", names)}");
