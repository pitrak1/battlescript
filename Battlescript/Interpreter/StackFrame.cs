namespace Battlescript;

public class StackFrame
{
    public string File { get; set; }
    public int Line { get; set; }
    public string Expression { get; set; }
    public string Function { get; set; }
    public Dictionary<string, Variable> Values { get; set; }
    
    public StackFrame(string file, int line, string expression, string function)
    {
        File = file;
        Line = line;
        Expression = expression;
        Function = function;
        Values = new Dictionary<string, Variable>();
    }
}