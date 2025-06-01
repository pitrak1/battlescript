namespace Battlescript;

public class InterpreterInvalidIndexException(Variable? index) : Exception(
    "Interpreter Error: Variable " + index + " is not a valid index value");