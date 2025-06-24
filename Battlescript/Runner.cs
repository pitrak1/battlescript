namespace Battlescript;

public static class Runner
{
    public static Memory Run(string input)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        Postparser.Run(parserResult);
        var interpreter = new Interpreter(parserResult);
        return interpreter.Run();
    }
}