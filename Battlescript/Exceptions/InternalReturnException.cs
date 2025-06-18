namespace Battlescript;

public class InternalReturnException(Variable? value = null)
    : Exception("This is just an internal exception to handle return statements.")
{
    public Variable? Value { get; set; } = value;
}