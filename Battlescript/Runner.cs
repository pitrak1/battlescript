namespace Battlescript;

public static class Runner
{
    public static List<Dictionary<string, Variable>> Run(string input)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        var postParser = new Postparser(parserResult);
        postParser.Run();
        var interpreter = new Interpreter(parserResult);
        return interpreter.Run();
    }
}