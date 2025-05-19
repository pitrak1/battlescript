using Battlescript;
using Microsoft.VisualBasic;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertTokenListEqual(List<Token> input, List<Token> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        for (var i = 0; i < input.Count; i++)
        {
            Assert.That(input[i], Is.EqualTo(expected[i]));
        }
    }

    public static void AssertInstructionListEqual(List<Instruction>? input, List<Instruction>? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else
        {
            Assert.That(expected, Is.Not.Null);
            Assert.That(input, Is.EquivalentTo(expected));
        }
    }
}