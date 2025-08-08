namespace Battlescript;

public static class Runner
{
    public static Memory Run(string input)
    {
        var memory = new Memory([new MemoryScope("builtin")]);
        
        foreach (var builtin in Memory.BsTypeStrings)
        {
            LoadBuiltin(memory, builtin);
            memory.PopulateBsTypeReference(builtin);
        }

        try
        {
            memory.AddScope(new MemoryScope("main"));
            RunPartial(memory, input, "main");
        }
        catch (InternalReturnException e)
        {
            memory.PrintStacktrace();
            throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "'return' outside function");
        }
        catch (Exception e)
        {
            memory.PrintStacktrace();
            throw;
        }
        
        return memory;
    }

    public static MemoryScope RunFilePath(Memory memory, string path)
    {
        var input = ReadFile(path);
        RunPartial(memory, input, path);
        return memory.RemoveScope();
    }

    private static void LoadBuiltin(Memory memory, string builtinName)
    {
        var fileName = $"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{builtinName}.bs";
        string text = ReadFile(fileName);
        RunPartial(memory, text, fileName);
    }

    private static string ReadFile(string path)
    {
        using StreamReader reader = new(path);
        return reader.ReadToEnd();
    }

    public static void RunPartial(Memory memory, string input, string fileName)
    {
        var lexer = new Lexer(input, fileName);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        Postparser.Run(parserResult);
        var interpreter = new Interpreter(parserResult);
        interpreter.Run(memory);
    }
}