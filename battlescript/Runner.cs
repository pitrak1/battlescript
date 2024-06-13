namespace BattleScript.Core;

public class Runner
{
    private string _basePath;
    private Interpreter? interpreter;


    public Runner(string basePath)
    {
        _basePath = basePath;
    }

    public void Load(string path)
    {
        string contents = ReadFile(path);

        Lexer lexer = new Lexer(contents);
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        interpreter = new Interpreter(instructions);
    }

    public Dictionary<string, Variable>[] Run()
    {
        return interpreter.Run();
    }

    public Dictionary<string, Variable>[] RunDebug()
    {
        return interpreter.RunDebug();
    }


    public Dictionary<string, Variable>[] Continue()
    {
        return interpreter.Continue();
    }


    private string ReadFile(string path)
    {
        path = $"{_basePath}/{path}";
        return File.ReadAllText(path);
    }
}