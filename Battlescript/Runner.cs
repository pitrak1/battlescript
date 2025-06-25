namespace Battlescript;

public static class Runner
{
    public static Memory Run(string input)
    {
        var memory = new Memory();
        RunPartial(memory, input);
        return memory;
    }

    private static void LoadBuiltin(Memory memory, string builtinName)
    {
        using StreamReader reader = new($"/Users/nickpitrak/Desktop/Battlescript/Battlescript/Builtins/{builtinName}.bs");
        string text = reader.ReadToEnd();
        RunPartial(memory, text);
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