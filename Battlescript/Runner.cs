namespace Battlescript;

public static class Runner
{
    public static Memory Run(string input)
    {
        var memory = new Memory();
        RunPartial(memory, input);
        return memory;
    }

    public static Dictionary<string, Variable> RunFilePath(string path)
    {
        var memory = new Memory();
        var input = ReadFile(path);
        RunPartial(memory, input);
        return memory.Scopes.First();
    }

    private static void LoadBuiltin(Memory memory, string builtinName)
    {
        string text = ReadFile(builtinName);
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