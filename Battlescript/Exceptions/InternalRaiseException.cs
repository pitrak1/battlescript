namespace Battlescript;

public class InternalRaiseException : Exception
{
    public string? Type { get; set; }
    
    public InternalRaiseException(string message) : base(message) {}
    
    public InternalRaiseException(BsTypes.Types type, string message) : base(message)
    {
        Type = BsTypes.TypesToStrings[type];
    }

    public InternalRaiseException(string? type, string message) : base(message)
    {
        Type = type;
    }
}