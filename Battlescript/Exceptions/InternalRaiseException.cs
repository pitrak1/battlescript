namespace Battlescript;

public class InternalRaiseException : Exception
{
    public Variable? Value { get; set; }
    public Memory.BsTypes Type { get; set; }

    public InternalRaiseException(Variable value) : base("")
    {
        Value = value;
    }

    public InternalRaiseException(Memory.BsTypes type, string message) : base(message)
    {
        Type = type;
    }
}