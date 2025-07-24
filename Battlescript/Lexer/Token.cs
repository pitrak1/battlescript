namespace Battlescript;

public class Token(Consts.TokenTypes type, string value, int? line = null, string? fileName = null, string? expression = null)
{    
    public Consts.TokenTypes Type { get; set; } = type;
    public string Value { get; set; } = value;
    public int? Line { get; set; } = line;
    public string? FileName { get; set; } = fileName;
    public string? Expression { get; set; } = expression;
}