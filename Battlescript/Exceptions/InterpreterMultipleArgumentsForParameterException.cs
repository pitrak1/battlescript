namespace Battlescript;

public class InterpreterMultipleArgumentsForParameterException(string parameterName) : Exception(
    $"Parameter {parameterName} is addressed by multiple arguments");