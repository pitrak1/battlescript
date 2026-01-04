namespace Battlescript;

public static class Postparser
{
    public static void Run(List<Instruction> instructions)
    {
        CheckForLackOfIndents(instructions);
        JoinIfElse(instructions);
        JoinTryExceptElseFinally(instructions);
    }

    private static void CheckForLackOfIndents(List<Instruction> instructions)
    {
        foreach (var t in instructions)
        {
            if (Parser.IsInstructionExpectingIndent(t) && t.Instructions.Count == 0)
            {
                throw new InternalRaiseException(BsTypes.Types.SyntaxError, "expected an indented block");
            }
            
            CheckForLackOfIndents(t.Instructions);
        }
    }

    private static void JoinIfElse(List<Instruction> instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            Instruction currentInstruction = instructions[i];
            
            if (instructions[i] is IfInstruction)
            {
                var nextInstruction = GetElseInstructionIfPresent(i + 1);
                while (nextInstruction is not null)
                {
                    // Add else instruction to the end of the chain of else's on the initial if instruction and
                    // then remove it from the starting instruction list
                    currentInstruction.Next = nextInstruction;
                    currentInstruction = nextInstruction;
                    instructions.RemoveAt(i + 1);
                    nextInstruction = GetElseInstructionIfPresent(i + 1);
                }
            }
            
            JoinIfElse(currentInstruction.Instructions);
        }

        return;

        ElseInstruction? GetElseInstructionIfPresent(int index)
        {
            if (index < instructions.Count && instructions[index] is ElseInstruction elseInstruction)
            {
                return elseInstruction;
            }

            return null;
        }
    }
    
    private static void JoinTryExceptElseFinally(List<Instruction> instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            if (instructions[i] is TryInstruction tryInstruction)
            {
                while (IsInstructionExceptElseOrFinally(i + 1))
                {
                    switch (instructions[i + 1])
                    {
                        case ExceptInstruction exceptInstruction:
                            tryInstruction.Excepts.Add(exceptInstruction);
                            break;
                        case ElseInstruction elseInstruction:
                            tryInstruction.Else = elseInstruction;
                            break;
                        case FinallyInstruction finallyInstruction:
                            tryInstruction.Finally = finallyInstruction;
                            break;
                    }

                    instructions.RemoveAt(i + 1);
                }
            }
            
            JoinTryExceptElseFinally(instructions[i].Instructions);
        }
        
        bool IsInstructionExceptElseOrFinally(int index)
        {
            return index < instructions.Count && instructions[index] is ExceptInstruction or ElseInstruction or FinallyInstruction;
        }
    }
}