namespace Battlescript;

public class Postparser(List<Instruction> instructions)
{
    public void Run()
    {
        JoinIfElse();
    }

    private void JoinIfElse()
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