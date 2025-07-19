using System.Diagnostics;

namespace Battlescript;

public class ExceptInstruction : Instruction
{
    public Instruction? Value { get; set; }

    public ExceptInstruction(List<Token> tokens)
    {
        // Want to ignore except keyword at start and colon at end
        Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
        Line = tokens[0].Line;
        Column = tokens[0].Column;
    }
    
    public ExceptInstruction(Instruction value, List<Instruction> instructions)
    {
        Value = value;
        Instructions = instructions;
    }
    
    public override Variable Interpret(
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
        
        return new ConstantVariable();
    }
}