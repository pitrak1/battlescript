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
            Assert.That(input.Value, Is.EqualTo(expected.Value));
            AssertInstructionEqual(input.Left, expected.Left);
            AssertInstructionEqual(input.Right, expected.Right);
        }
    }
}