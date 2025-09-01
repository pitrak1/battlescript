namespace Battlescript;

public class Token(Consts.TokenTypes type, string value, int? line = null, string? fileName = null, string? expression = null)
{    
    public Consts.TokenTypes Type { get; set; } = type;
    public string Value { get; set; } = value;
    public int Line { get; set; } = line ?? -1;
    public string FileName { get; set; } = fileName ?? "main";
    public string Expression { get; set; } = expression ?? "";
}