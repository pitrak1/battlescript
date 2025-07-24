namespace Battlescript;

public class MemoryScope
{
    public Dictionary<string, Variable> Variables { get; set; } = new();
    public string? FileName { get; set; }
    public int? LineNumber { get; set; }
    public string? FunctionName { get; set; }
    public string? Expression { get; set; }

    public MemoryScope(
        Dictionary<string, Variable>? variables = null, 
        string? fileName = null, 
        int? lineNumber = null,
        string? functionName = null,
        string? expression = null)
    {
        Variables = variables ?? new();
        FileName = fileName;
        LineNumber = lineNumber;
        FunctionName = functionName;
        Expression = expression;
    }
    public void Add(string name, Variable variable)
    {
        Variables.Add(name, variable);
    }
}