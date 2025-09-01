namespace Battlescript;

public class InternalRaiseException : Exception
{
    public Memory.BsTypes? Type { get; set; }

    public InternalRaiseException(string message) : base(message)
    {
    }

    public InternalRaiseException(Memory.BsTypes type, string message) : base(message)
    {
        Type = type;
    }
}