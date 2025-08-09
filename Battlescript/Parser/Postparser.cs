namespace Battlescript;

public static class Postparser
{
    public static void Run(List<Instruction> instructions)
    {
        // NOTE that these functions need to be changed to be recursive.  Right now we're just checking
        // for instruction types in the outermost scope
        CheckForLackOfIndents(instructions);
        JoinIfElse(instructions);
        JoinTryExceptElseFinally(instructions);
    }

    private static void CheckForLackOfIndents(List<Instruction> instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            if (Consts.InstructionTypesExpectingIndent.Contains(instructions[i].GetType().Name) &&
                instructions[i].Instructions.Count == 0)
            {
                throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "expected an indented block");
            }
        }
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
    
    private static void JoinTryExceptElseFinally(List<Instruction> instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            if (instructions[i] is TryInstruction tryInstruction)
            {
                while (i + 1 < instructions.Count && instructions[i + 1] is ExceptInstruction or ElseInstruction or FinallyInstruction)
                {
                    if (instructions[i + 1] is ExceptInstruction exceptInstruction)
                    {
                        tryInstruction.Excepts.Add(exceptInstruction);
                    } else if (instructions[i + 1] is ElseInstruction elseInstruction)
                    {
                        tryInstruction.Else = elseInstruction;
                    } else if (instructions[i + 1] is FinallyInstruction finallyInstruction)
                    {
                        tryInstruction.Finally = finallyInstruction;
                    }
                    
                    instructions.RemoveAt(i + 1);
                }
            }
        }
    }
}