using Battlescript;

namespace BattlescriptTests;

public class E2EAssertions
{
    public static void AssertVariableValueFromInput(string input, string name, Variable? expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        var interpreter = new Interpreter(parserResult);
        var interpreterResult = interpreter.Run();

        Assert.That(interpreterResult[0], Contains.Key(name));
        Assert.That(interpreterResult[0][name], Is.EqualTo(expected));
    }
}