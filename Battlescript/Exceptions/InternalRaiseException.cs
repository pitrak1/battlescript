namespace Battlescript;

public class InternalRaiseException(Variable? value = null)
    : Exception("This is just an internal exception to handle raise statements.")
{
    public Variable? Value { get; set; } = value;
}