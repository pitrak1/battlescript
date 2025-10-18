using System.Diagnostics;

namespace Battlescript;

public class ExceptInstruction : Instruction
{
    public Instruction? Value { get; set; }
    public VariableInstruction? ExceptionVariable { get; set; }

    public ExceptInstruction(List<Token> tokens) : base(tokens)
    {
        var asIndex = InstructionUtilities.GetTokenIndex(tokens, ["as"]);
        if (asIndex == -1)
        {
            // Want to ignore except keyword at start and colon at end
            Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2));
        }
        else
        {
            Value = InstructionFactory.Create(tokens.GetRange(1, asIndex - 1));
            ExceptionVariable = InstructionFactory.Create(tokens.GetRange(asIndex + 1, tokens.Count - asIndex - 1)) as VariableInstruction;
        }
    }
    
    public ExceptInstruction(Instruction value, List<Instruction> instructions, VariableInstruction? exceptionVariable = null) : base([])
    {
        Value = value;
        Instructions = instructions;
        ExceptionVariable = exceptionVariable;
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