namespace Battlescript;

public class InterpreterInvalidOperationException(string operation, Variable? left, Variable? right) : Exception(
    "Interpreter Error: Operation " + operation + " is invalid with operands " + left + " and " + right);