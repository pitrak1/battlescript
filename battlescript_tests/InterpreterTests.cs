using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Tests;

namespace BattleScript.InterpreterTests;

public class InterpreterTests
{
    [Test]
    public void NumberLiteral()
    {
        Lexer lexer = new Lexer("var x = 5;");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        var result = interpreter.Run();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.Number, 5)}
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void StringLiteral()
    {
        Lexer lexer = new Lexer("var x = 'asdf';");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        var result = interpreter.Run();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.String, "asdf")}
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void BooleanLiteral()
    {
        Lexer lexer = new Lexer("var x = true;");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        var result = interpreter.Run();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.Boolean, true)}
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}