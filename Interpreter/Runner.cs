namespace BattleScript; 

public class Runner {
    private string? _basePath;
    private CustomCallbacks? _callbacks;

    public Runner(string basePath, CustomCallbacks? callbacks = null) {
        _basePath = basePath;
        if (callbacks == null) {
            _callbacks = new CustomCallbacks();
        }
        else {
            _callbacks = callbacks;
        }
    }

    public ScopeStack Run(string path) {
        string contents = ReadFile(path);
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        var interpreter = new Interpreter(_callbacks);
        return interpreter.Run(instructions);
    }

    public ScopeStack RunString(string contents) {
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        var interpreter = new Interpreter(_callbacks);
        return interpreter.Run(instructions);
    }

    private string ReadFile(string path) {
        if (_basePath is not null) {
            path = $"{_basePath}/{path}";
        }
        
        return File.ReadAllText(path);
    }
}