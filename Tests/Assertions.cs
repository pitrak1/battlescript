namespace BattleScript.Tests; 

public class Assertions {
    public static void AssertTokens(List<Token> tokens, List<Token> expectedTokens) {
        for (int i = 0; i < expectedTokens.Count; i++) {
            Console.WriteLine($"{expectedTokens[i].Type}, {expectedTokens[i].Value}");
            Console.WriteLine($"{tokens[i].Type}, {tokens[i].Value}");
            Assert.That(expectedTokens[i] == tokens[i]);
        }
    }

    public static void AssertInstructions(List<Instruction> instructions, List<Instruction> expectedInstructions) {
        for (int i = 0; i < expectedInstructions.Count; i++) {
            AssertInstruction(instructions[i], expectedInstructions[i]);
        }
    }

    private static void AssertInstruction(Instruction instruction, Instruction expected) {
        if (instruction is null != expected is null) {
            Assert.Fail("Comparing against null object");
        }

        if (instruction is null && expected is null) {
            return;
        }
        
        Assert.That(instruction, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        
        Assert.That(instruction.Type, Is.EqualTo(expected.Type));
        Assert.That(instruction.Operator, Is.EqualTo(expected.Operator));
        
        AssertInstruction(instruction.Left, expected.Left);
        AssertInstruction(instruction.Right, expected.Right);

        Assert.That(instruction.IntegerValue, Is.EqualTo(expected.IntegerValue));
        Assert.That(instruction.StringValue, Is.EqualTo(expected.StringValue));
        Assert.That(instruction.BoolValue, Is.EqualTo(expected.BoolValue));
        
        AssertInstruction(instruction.InstructionValue, expected.InstructionValue);
        
        Assert.That(instruction.InstructionListValue.Count, Is.EqualTo(expected.InstructionListValue.Count));
        for (int i = 0; i < instruction.InstructionListValue.Count; i++) {
            AssertInstruction(instruction.InstructionListValue[i], expected.InstructionListValue[i]);
        }
        
        Assert.That(instruction.Instructions.Count, Is.EqualTo(expected.Instructions.Count));
        for (int i = 0; i < instruction.Instructions.Count; i++) {
            AssertInstruction(instruction.Instructions[i], expected.Instructions[i]);
        }
    }
}