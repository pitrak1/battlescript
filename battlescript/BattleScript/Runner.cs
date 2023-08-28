using BattleScript.LexerNS;
using BattleScript.ParserNS;
using BattleScript.InterpreterNS;

namespace BattleScript.Core;

public class Runner
{
    private string? _basePath;

    public Runner(string basePath)
    {
        _basePath = basePath;
    }

    public ScopeStack Run(string path)
    {
        string contents = ReadFile(path);

        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);

        Parser parser = new Parser();
        var instructions = parser.Run(tokens);

        var interpreter = new Interpreter();
        return interpreter.Run(instructions);
    }

    public ScopeStack RunString(string contents)
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);

        Parser parser = new Parser();
        var instructions = parser.Run(tokens);

        var interpreter = new Interpreter();
        return interpreter.Run(instructions);
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