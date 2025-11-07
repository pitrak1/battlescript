namespace Battlescript;

public class CallStack
{
    public List<StackFrame> Frames { get; set; }

    public CallStack()
    {
        Frames = [new StackFrame("main", "<module>")];
    }
    
    public void AddFrame(int entryLine, string entryExpression, string function, string? file = null)
    {
        Frames[^1].UpdateLineAndExpression(entryLine, entryExpression);
        var fileValue = file ?? Frames[^1].File;
        Frames.Add(new StackFrame(fileValue, function));
    }

    public StackFrame RemoveFrame()
    {
        var removedScope = Frames[^1];
        Frames.RemoveAt(Frames.Count - 1);
        return removedScope;
    }
}