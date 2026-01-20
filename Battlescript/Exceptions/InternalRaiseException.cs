namespace Battlescript;

public class InternalRaiseException : Exception
{
    public string? Type { get; set; }
    
    public InternalRaiseException(string message) : base(message) {}
    
    public InternalRaiseException(BtlTypes.Types type, string message) : base(message)
    {
        Type = BtlTypes.TypesToStrings[type];
    }

    public InternalRaiseException(string? type, string message) : base(message)
    {
        Type = type;
    }
}