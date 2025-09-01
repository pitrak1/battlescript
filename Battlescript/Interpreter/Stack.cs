using System.Linq.Expressions;

namespace Battlescript;

public class Stack
{
    public List<(string File, int Line, string Expression, string Function)> Frames = new();

    public List<string> Files = [];
    public List<string> Functions = [];

    public (bool file, bool function) AddFrame(Instruction inst, string? file = null, string? function = null)
    {
        Frames.Add((Files[^1], inst.Line, inst.Expression, Functions[^1]));
        var fileUpdated = false;
        if (file is not null)
        {
            Files.Add(file);
            fileUpdated = true;
        }
        
        var functionUpdated = false;
        if (function is not null)
        {
            Functions.Add(function);
            functionUpdated = true;
        }
        
        return (fileUpdated, functionUpdated);
    }

    public void PopFrame((bool file, bool function) status)
    {
        if (status.function) Functions.RemoveAt(Functions.Count - 1);
        if (status.file) Files.RemoveAt(Files.Count - 1);
        Frames.RemoveAt(Frames.Count - 1);
    }

    public void PrintStacktrace()
    {
        Console.WriteLine("Traceback (most recent call last):");
        foreach (var frame in Frames)
        {
            Console.WriteLine($"\tFile {frame.File}, line {frame.Line}, in {frame.Function}");
            Console.WriteLine("\t" + frame.Expression);
        }
    }
}