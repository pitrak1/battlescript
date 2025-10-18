using System.Diagnostics;

namespace Battlescript;

public class ExceptInstruction : Instruction
{
    public VariableInstruction? ExceptionType { get; set; }
    public VariableInstruction? ExceptionVariable { get; set; }

    public ExceptInstruction(List<Token> tokens) : base(tokens)
    {
        var asIndex = InstructionUtilities.GetTokenIndex(tokens, ["as"]);
        if (asIndex == -1)
        {
            // Want to ignore except keyword at start and colon at end
            ExceptionType = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 2)) as VariableInstruction;
        }
        else
        {
            ExceptionType = InstructionFactory.Create(tokens.GetRange(1, asIndex - 1)) as VariableInstruction;
            ExceptionVariable = InstructionFactory.Create(tokens.GetRange(asIndex + 1, tokens.Count - asIndex - 2)) as VariableInstruction;
        }
    }
    
    public ExceptInstruction(VariableInstruction exceptionType, List<Instruction> instructions, VariableInstruction? exceptionVariable = null) : base([])
    {
        ExceptionType = exceptionType;
        Instructions = instructions;
        ExceptionVariable = exceptionVariable;
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        return null;
    }
}