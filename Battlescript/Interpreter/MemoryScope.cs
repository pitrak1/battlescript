namespace Battlescript;

public class MemoryScope
{
    public Dictionary<string, Variable> Variables { get; set; } = new();
    public string? FileName { get; set; }
    public int? Line { get; set; }
    public string? FunctionName { get; set; }
    public string? Expression { get; set; }

    public MemoryScope(Dictionary<string, Variable>? variables = null)
    {
        Variables = variables ?? new();
    }

    public MemoryScope(
        string? fileName = null,
        int? line = null,
        string? functionName = null,
        string? expression = null)
    {
        Variables = new();
        FileName = fileName;
        Line = line;
        FunctionName = functionName;
        Expression = expression;
    }
    public void Add(string name, Variable variable)
    {
        Variables.Add(name, variable);
    }
}