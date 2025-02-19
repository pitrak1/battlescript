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
        AssertVariableEqual(interpreterResult[0][name], expected);
    }
    
    private static void AssertVariableEqual(Variable? input, Variable? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else
        {
            Assert.That(expected, Is.Not.Null);
            if (input.Value is List<Variable>)
            {
                AssertVariableListEqual(input.Value, expected.Value);
            }
            else if (input.Value is Dictionary<string, Variable>)
            {
                AssertVariableDictionaryEqual(input.Value, expected.Value);
            }
            else
            {
                Assert.That(input.Value, Is.EqualTo(expected.Value));
            }
            Assert.That(input.Type, Is.EqualTo(expected.Type));
        }
    }
    
    private static void AssertVariableListEqual(List<Variable>? input, List<Variable>? expected)
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        for (var i = 0; i < input.Count; i++)
        {
            AssertVariableEqual(input[i], expected[i]);
        }
    }
    
    private static void AssertVariableDictionaryEqual(
        Dictionary<string, Variable>? input, 
        Dictionary<string, Variable>? expected
    )
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        foreach (var kvp in expected)
        {
            Assert.That(input, Contains.Key(kvp.Key));
            AssertVariableEqual(input[kvp.Key], expected[kvp.Key]);
        }
    }
}