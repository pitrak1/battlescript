using System.Diagnostics;

namespace Battlescript;

public class ExceptInstruction : Instruction
{
    public VariableInstruction? ExceptionType { get; set; }
    public VariableInstruction? ExceptionVariable { get; set; }

    public ExceptInstruction(List<Token> tokens) : base(tokens)
    {
        ExceptionType = new VariableInstruction(tokens[1].Value);
        
        if (tokens.Count > 3)
        {
            ExceptionVariable = new VariableInstruction(tokens[3].Value);
        }
    }

    public ExceptInstruction(VariableInstruction exceptionType, List<Instruction> instructions, VariableInstruction? exceptionVariable = null) : base([])
    {
        ExceptionType = exceptionType;
        Instructions = instructions;
        ExceptionVariable = exceptionVariable;
    }
}