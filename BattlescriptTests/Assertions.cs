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
            Assert.That(input.LiteralValue, Is.EqualTo(expected.LiteralValue));
            Assert.That(input.Name, Is.EqualTo(expected.Name));
            Assert.That(input.Operation, Is.EqualTo(expected.Operation));
            AssertInstructionEqual(input.Value, expected.Value);
            AssertInstructionListEqual(input.ValueList, expected.ValueList);
            AssertInstructionEqual(input.Next, expected.Next);
            AssertInstructionEqual(input.Left, expected.Left);
            AssertInstructionEqual(input.Right, expected.Right);

            if (input.Instructions.Count > 0)
            {
                AssertInstructionListEqual(input.Instructions, expected.Instructions);
            }
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

    
}