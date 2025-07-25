using System.Diagnostics;

namespace Battlescript;

public class RaiseInstruction : Instruction
{
    public Instruction? Value { get; set; }

    public RaiseInstruction(List<Token> tokens) : base(tokens)
    {
        Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 1));
    }
    
    public override Variable? Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var exception = Value?.Interpret(memory);
        throw new InternalRaiseException(exception);
    }
}