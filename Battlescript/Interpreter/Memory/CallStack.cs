namespace Battlescript;

public class CallStack
{
    public List<StackFrame> Scopes { get; set; }

    public CallStack()
    {
        Scopes = [new StackFrame("main", "<module>")];
    }
    
    public void AddScope(int entryLine, string entryExpression, string function, string? file = null)
    {
        Scopes[^1].UpdateLineAndExpression(entryLine, entryExpression);
        var fileValue = file ?? Scopes[^1].File;
        Scopes.Add(new StackFrame(fileValue, function));
    }

    public StackFrame RemoveScope()
    {
        var removedScope = Scopes[^1];
        Scopes.RemoveAt(Scopes.Count - 1);
        return removedScope;
    }
}