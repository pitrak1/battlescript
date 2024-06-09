namespace BattleScript.Core;

public class Runner
{
    private string? _basePath;

    public Runner(string basePath)
    {
        _basePath = basePath;
    }

    public Dictionary<string, Variable> Run(string path)
    {
        string contents = ReadFile(path);

        Lexer lexer = new Lexer(contents);
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        var interpreter = new Interpreter(instructions);
        return interpreter.Run();
    }

    public Dictionary<string, Variable> RunString(string contents)
    {
        Lexer lexer = new Lexer(contents);
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        var interpreter = new Interpreter(instructions);
        return interpreter.Run();
    }

    private string ReadFile(string path)
    {
        if (_basePath is not null)
        {
            path = $"{_basePath}/{path}";
        }

        return File.ReadAllText(path);
    }
}