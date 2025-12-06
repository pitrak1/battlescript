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
        PrintStacktrace();
    }

    public StackFrame RemoveFrame()
    {
        var removedScope = Frames[^1];
        Frames.RemoveAt(Frames.Count - 1);
        return removedScope;
    }

    public void PrintStacktrace()
    {
        Console.WriteLine("Stacktrace:");
        for (var i = Frames.Count - 1; i >= 0; i--)
        {
            Console.WriteLine($"File {Frames[i].File}, Function {Frames[i].Function}, Line {Frames[i].Line}, Expression {Frames[i].Expression}");
        }
        Console.WriteLine();
    }
}