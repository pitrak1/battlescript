namespace Battlescript;

public class MemoryScope
{
    public Dictionary<string, Variable> Variables { get; set; } = new();
    public string? FileName { get; set; }
    public int LineNumber { get; set; }
    public string FunctionName { get; set; }

    public MemoryScope(Dictionary<string, Variable>? variables = null)
    {
        Variables = variables ?? new();
    }
    public void Add(string name, Variable variable)
    {
        Variables.Add(name, variable);
    }
}