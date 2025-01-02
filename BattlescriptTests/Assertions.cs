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
        for (int i = 0; i < input.Count; i++)
        {
            AssertTokenEqual(input[i], expected[i]);
        }
    }
}