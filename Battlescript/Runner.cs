namespace Battlescript;

public static class Runner
{
    public static List<Dictionary<string, Variable>> Run(string input)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        var interpreter = new Interpreter(parserResult);
        return interpreter.Run();
    }
}