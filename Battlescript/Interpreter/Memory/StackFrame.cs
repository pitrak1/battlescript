namespace Battlescript;

public class StackFrame
{
    public string File { get; set; }
    public int? Line { get; set; }
    public string? Expression { get; set; }
    public string Function { get; set; }
    public Dictionary<string, Variable> Values { get; set; }
    
    public StackFrame(string file, string function)
    {
        File = file;
        Function = function;
        Values = new Dictionary<string, Variable>();
    }

    public StackFrame(string file, int line, string expression, string function) : this(file, function)
    {
        Line = line;
        Expression = expression;
    }

    public void UpdateLineAndExpression(int line, string expression)
    {
        Line = line;
        Expression = expression;
    }
}