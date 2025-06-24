namespace Battlescript;

public static class Postparser
{
    public static void Run(List<Instruction> instructions)
    {
        JoinIfElse(instructions);
    }

    private static void JoinIfElse(List<Instruction> instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            if (instructions[i] is IfInstruction)
            {
                Instruction currentInstruction = instructions[i];
                while (i + 1 < instructions.Count && instructions[i + 1] is ElseInstruction elseInstruction)
                {
                    if (currentInstruction is IfInstruction ifInstruction)
                    {
                        ifInstruction.Next = elseInstruction;
                    } else
                    {
                        ((ElseInstruction)currentInstruction).Next = elseInstruction;
                    }

                    currentInstruction = elseInstruction;
                    instructions.RemoveAt(i + 1);
                }
            }
        }
    }
}