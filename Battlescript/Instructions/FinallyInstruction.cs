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
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        memory.AddScope();
        foreach (var inst in Instructions)
        {
            inst.Interpret(memory);
        }
        memory.RemoveScope();

        return null;
    }
}