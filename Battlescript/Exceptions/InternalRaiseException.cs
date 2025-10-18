namespace Battlescript;

public class InternalRaiseException : Exception
{
    public string? Type { get; set; }
    
    public InternalRaiseException(string message) : base(message) {}
    
    public InternalRaiseException(Memory.BsTypes type, string message) : base(message)
    {
        Type = Memory.BsTypesToStrings[type];
    }

    public InternalRaiseException(string? type, string message) : base(message)
    {
        Type = type;
    }
}