using BattleScript.Core;

namespace BattleScript.DebugTests;

public class DebugTests
{
    [Test]
    public void RunIgnoresBreakpoints()
    {
        Lexer lexer = new Lexer(@"
            var x = 5;
            breakpoint;
            var y = 6;
        ");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        var result = interpreter.Run();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.Number, 5)},
            {"y", new Variable(Consts.VariableTypes.Number, 6)}
        };
        Assert.That(result[0], Is.EqualTo(expectedResult));
    }

    [Test]
    public void RunDebugRespectsBreakpoints()
    {
        Lexer lexer = new Lexer(@"
            var x = 5;
            breakpoint;
            var y = 6;
        ");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        var result = interpreter.RunDebug();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.Number, 5)}
        };
        Assert.That(result[0], Is.EqualTo(expectedResult));
    }

    [Test]
    public void ContinueRespectsBreakpoints()
    {
        Lexer lexer = new Lexer(@"
            var x = 5;
            breakpoint;
            var y = 6;
            breakpoint;
            var z = 7;
        ");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        interpreter.RunDebug();
        var result = interpreter.Continue();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.Number, 5)},
            {"y", new Variable(Consts.VariableTypes.Number, 6)}
        };
        Assert.That(result[0], Is.EqualTo(expectedResult));
    }
}