using Battlescript;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertInputProducesParserOutput(string input, Instruction? expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        
        Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
    }

    public static void AssertInputProducesValueInMemory(string input, string variableName, Variable? expected)
    {
        var memory = Runner.Run(input);
        
        Assert.That(memory.Scopes[0], Contains.Key(variableName));
        Assert.That(memory.Scopes[0][variableName], Is.EqualTo(expected));
    }
}