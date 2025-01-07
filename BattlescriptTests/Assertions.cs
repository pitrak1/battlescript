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

    private static void AssertInstructionListEqual(List<Instruction>? input, List<Instruction>? expected)
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
}