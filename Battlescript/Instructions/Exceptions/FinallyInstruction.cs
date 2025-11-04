using System.Diagnostics;

namespace Battlescript;

public class FinallyInstruction : Instruction
{
    public FinallyInstruction(List<Token> tokens) : base(tokens) {}

    public FinallyInstruction(List<Instruction> instructions) : base([])
    {
        Instructions = instructions;
    }
    
    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        foreach (var inst in Instructions)
        {
            inst.Interpret(callStack, closure);
        }

        return null;
    }
}