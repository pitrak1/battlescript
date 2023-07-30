using System.Text.Json;
using Newtonsoft.Json;
using BattleScript.Core;

namespace BattleScript.Tests;

public class Assertions
{
    public static void AssertTokens(List<Token> tokens, List<Token> expectedTokens)
    {
        for (int i = 0; i < expectedTokens.Count; i++)
        {
            AssertToken(tokens[i], expectedTokens[i]);
        }
    }

    private static void AssertToken(Token token, Token expected)
    {
        Assert.That(token.Type, Is.EqualTo(expected.Type));
        Assert.That(token.Value, Is.EqualTo(expected.Value));
    }

    public static void AssertInstructions(List<Instruction> instructions, List<Instruction> expectedInstructions)
    {
        List<string> path = new List<string>();
        Assert.That(instructions.Count, Is.EqualTo(expectedInstructions.Count));
        for (int i = 0; i < expectedInstructions.Count; i++)
        {
            path.Add(i.ToString());
            AssertInstruction(instructions[i], expectedInstructions[i], path);
            path.RemoveAt(path.Count - 1);
        }
    }

    private static void AssertInstruction(Instruction instruction, Instruction expected, List<string> path)
    {
        if (instruction is null != expected is null)
        {
            Assert.Fail("Comparing against null object");
        }

        if (instruction is null && expected is null)
        {
            return;
        }

        Assert.That(instruction.Type, Is.EqualTo(expected.Type), GetErrorString("Type", expected.Type, instruction.Type, path));

        path.Add("left");
        AssertInstruction(instruction.Left, expected.Left, path);
        path.RemoveAt(path.Count - 1);

        path.Add("right");
        AssertInstruction(instruction.Right, expected.Right, path);
        path.RemoveAt(path.Count - 1);

        path.Add("next");
        AssertInstruction(instruction.Next, expected.Next, path);
        path.RemoveAt(path.Count - 1);

        path.Add("value");
        if (expected.Value is string || expected.Value is int || expected.Value is bool)
        {
            Assert.That(instruction.Value, Is.EqualTo(expected.Value), GetErrorString("Value", expected.Value, instruction.Value, path));
        }
        else if (expected.Value is Instruction)
        {
            AssertInstruction(instruction.Value, expected.Value, path);
        }
        else if (expected.Value is List<Instruction>)
        {
            Assert.That(instruction.Value.Count, Is.EqualTo(expected.Value.Count), GetErrorString("Value.Count", expected.Value.Count, instruction.Value.Count, path));
            for (int i = 0; i < instruction.Value.Count; i++)
            {
                path.Add(i.ToString());
                AssertInstruction(instruction.Value[i], expected.Value[i], path);
                path.RemoveAt(path.Count - 1);
            }
        }
        path.RemoveAt(path.Count - 1);

        Assert.That(instruction.Instructions.Count, Is.EqualTo(expected.Instructions.Count), GetErrorString("Instructions.Count", expected.Instructions.Count, instruction.Instructions.Count, path));
        path.Add("instructions");
        for (int i = 0; i < instruction.Instructions.Count; i++)
        {
            path.Add(i.ToString());
            AssertInstruction(instruction.Instructions[i], expected.Instructions[i], path);
            path.RemoveAt(path.Count - 1);
        }
        path.RemoveAt(path.Count - 1);
    }

    private static string GetErrorString(string property, dynamic expected, dynamic actual, List<string> path)
    {
        string result = $"expected {property} {expected}, got {actual} (";
        foreach (string entry in path)
        {
            result += entry + ".";
        }
        result += ")";
        return result;
    }

    public static void AssertScope(
        Dictionary<string, ScopeVariable> scope,
        Dictionary<string, ScopeVariable> expected
    )
    {
        Assert.That(scope.Keys.Count, Is.EqualTo(expected.Keys.Count));
        foreach (KeyValuePair<string, ScopeVariable> entry in expected)
        {
            Assert.That(scope, Contains.Key(entry.Key), $"expected {entry.Key} to exist");

            ScopeVariable expectedValue = entry.Value;
            ScopeVariable actualValue = scope[entry.Key];
            Assert.That(actualValue.Type, Is.EqualTo(expectedValue.Type), $"expected {entry.Key} to be of type {expectedValue.Type}, got {actualValue.Type}");
            if (expectedValue.Value is int || expectedValue.Value is string || expectedValue.Value is bool)
            {
                Assert.That(actualValue.Value, Is.EqualTo(expectedValue.Value), $"expected {entry.Key} to equal {expectedValue.Value}, got {actualValue.Value}");
            }
        }
    }
}