namespace BattleScript.Tests; 

public class Assertions {
    public static void AssertTokens(List<Token> tokens, List<Token> expectedTokens) {
        for (int i = 0; i < expectedTokens.Count; i++) {
            AssertToken(tokens[i], expectedTokens[i]);
        }
    }

    private static void AssertToken(Token token, Token expected) {
        Assert.That(token.Type, Is.EqualTo(expected.Type));
        Assert.That(token.Value, Is.EqualTo(expected.Value));
    }

    public static void AssertInstructions(List<Instruction> instructions, List<Instruction> expectedInstructions) {
        List<string> path = new List<string>();
        for (int i = 0; i < expectedInstructions.Count; i++) {
            path.Add(i.ToString());
            AssertInstruction(instructions[i], expectedInstructions[i], path);
            path.RemoveAt(path.Count - 1);
        }
    }

    private static void AssertInstruction(Instruction instruction, Instruction expected, List<string> path) {
        if (instruction is null != expected is null) {
            Assert.Fail("Comparing against null object");
        }

        if (instruction is null && expected is null) {
            return;
        }
        
        Assert.That(instruction.Type, Is.EqualTo(expected.Type), GetErrorString("Type", expected.Type, instruction.Type, path));
        Assert.That(instruction.Operator, Is.EqualTo(expected.Operator), GetErrorString("Operator", expected.Operator, instruction.Operator, path));
        
        path.Add("left");
        AssertInstruction(instruction.Left, expected.Left, path);
        path.RemoveAt(path.Count - 1);
        
        path.Add("right");
        AssertInstruction(instruction.Right, expected.Right, path);
        path.RemoveAt(path.Count - 1);

        Assert.That(instruction.IntegerValue, Is.EqualTo(expected.IntegerValue), GetErrorString("IntegerValue", expected.IntegerValue, instruction.IntegerValue, path));
        Assert.That(instruction.StringValue, Is.EqualTo(expected.StringValue), GetErrorString("StringValue", expected.StringValue, instruction.StringValue, path));
        Assert.That(instruction.BoolValue, Is.EqualTo(expected.BoolValue), GetErrorString("BoolValue", expected.BoolValue, instruction.BoolValue, path));
        
        path.Add("instructionValue");
        AssertInstruction(instruction.InstructionValue, expected.InstructionValue, path);
        path.RemoveAt(path.Count - 1);
        
        Assert.That(instruction.InstructionListValue.Count, Is.EqualTo(expected.InstructionListValue.Count), GetErrorString("InstructionListValue.Count", expected.InstructionListValue.Count, instruction.InstructionListValue.Count, path));
        path.Add("instructionListValue");
        for (int i = 0; i < instruction.InstructionListValue.Count; i++) {
            path.Add(i.ToString());
            AssertInstruction(instruction.InstructionListValue[i], expected.InstructionListValue[i], path);
            path.RemoveAt(path.Count - 1);
        }
        path.RemoveAt(path.Count - 1);
        
        Assert.That(instruction.Instructions.Count, Is.EqualTo(expected.Instructions.Count), GetErrorString("Instructions.Count", expected.Instructions.Count, instruction.Instructions.Count, path));
        path.Add("instructions");
        for (int i = 0; i < instruction.Instructions.Count; i++) {
            path.Add(i.ToString());
            AssertInstruction(instruction.Instructions[i], expected.Instructions[i], path);
            path.RemoveAt(path.Count - 1);
        }
        path.RemoveAt(path.Count - 1);
    }

    private static string GetErrorString(string property, dynamic expected, dynamic actual, List<string> path) {
        string result = $"expected {property} {expected}, got {actual} (";
        foreach (string entry in path) {
            result += entry + ".";
        }
        result += ")";
        return result;
    }
}