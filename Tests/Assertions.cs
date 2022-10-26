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
            Console.WriteLine(expectedInstructions[i].Type);
            Console.WriteLine(instructions[i].Type);
            Assert.That(expectedInstructions[i] == instructions[i]);
        }
    }
}