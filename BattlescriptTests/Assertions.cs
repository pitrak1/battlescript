using Battlescript;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertTokenEqual(Token input, Token expected)
    {
        Assert.That(input.Type, Is.EqualTo(expected.Type));
        Assert.That(input.Value, Is.EqualTo(expected.Value));
    }

    public static void AssertTokenListEqual(List<Token> input, List<Token> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        for (var i = 0; i < input.Count; i++)
        {
            AssertTokenEqual(input[i], expected[i]);
        }
    }

    public static void AssertInstructionEqual(Instruction? input, Instruction? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else
        { ;
            Assert.That(expected, Is.Not.Null);
            
            Assert.That(input.Type, Is.EqualTo(expected.Type));

            if (input.Value is List<Instruction>)
            {
                AssertInstructionListEqual(input.Value, expected.Value);
            }
            else if (input.Value is List<(Instruction, Instruction)>)
            {
                AssertKeyValuePairListEqual(input.Value, expected.Value);
            }
            else
            {
                Assert.That(input.Value, Is.EqualTo(expected.Value));
            }
            AssertInstructionEqual(input.Left, expected.Left);
            AssertInstructionEqual(input.Right, expected.Right);
        }
    }

    public static void AssertInstructionListEqual(List<Instruction>? input, List<Instruction>? expected)
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        for (var i = 0; i < input.Count; i++)
        {
            AssertInstructionEqual(input[i], expected[i]);
        }
    }
    
    private static void AssertKeyValuePairListEqual(
        List<(Instruction Key, Instruction Value)>? input, 
        List<(Instruction Key, Instruction Value)>? expected
    )
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        for (var i = 0; i < input.Count; i++)
        {
            AssertInstructionEqual(input[i].Key, expected[i].Key);
            AssertInstructionEqual(input[i].Value, expected[i].Value);
        }
    }

    public static void AssertScopeListEqual(
        List<Dictionary<string, Variable>>? input,
        List<Dictionary<string, Variable>>? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else
        {
            Assert.That(expected, Is.Not.Null);
            Assert.That(input.Count, Is.EqualTo(expected.Count));

            for (var i = 0; i < input.Count; i++)
            {
                AssertScopesEqual(input[i], expected[i]);
            }
        }
    }

    private static void AssertScopesEqual(Dictionary<string, Variable>? input, Dictionary<string, Variable>? expected)
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        
        foreach (var kvp in expected)
        {
            Assert.That(input, Contains.Key(kvp.Key));
            if (input[kvp.Key] is Variable)
            {
                AssertVariableEqual(input[kvp.Key], expected[kvp.Key]);
            }
            else
            {
                Assert.That(input[kvp.Key], Is.EqualTo(kvp.Value));
            }
        }
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