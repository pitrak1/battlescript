namespace Battlescript;

public static class Runner
{
    public static Memory Run(string input)
    {
        var memory = new Memory();
        
        foreach (var builtin in BsTypes.TypeStrings)
        {
            LoadBuiltin(memory, builtin);
        }

        memory.PopulateBuiltInReferences();
        RunPartial(memory, input);
        return memory;
    }

    public static Dictionary<string, Variable> RunFilePath(Memory memory, string path)
    {
        var input = ReadFile(path);
        RunPartial(memory, input);
        return memory.Scopes.First();
    }

    private static void LoadBuiltin(Memory memory, string builtinName)
    {
        string text = ReadFile($"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{builtinName}.bs");
        RunPartial(memory, text);
    }

    private static string ReadFile(string path)
    {
        using StreamReader reader = new(path);
        return reader.ReadToEnd();
    }

    private static void RunPartial(Memory memory, string input)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        Postparser.Run(parserResult);
        var interpreter = new Interpreter(parserResult);
        interpreter.Run(memory);
    }
}