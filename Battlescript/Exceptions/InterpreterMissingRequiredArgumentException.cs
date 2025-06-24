namespace Battlescript;

public class InterpreterMissingRequiredArgumentException(string parameterName) : Exception(
    $"Required parameter {parameterName} is missing argument");