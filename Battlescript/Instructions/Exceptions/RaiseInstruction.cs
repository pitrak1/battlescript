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
        memory.CurrentStack.AddFrame(this);

        var exceptionType = memory.IsException(exception);
        if (exceptionType is Memory.BsTypes bsType)
        {
            throw new InternalRaiseException(bsType, memory.GetErrorMessage(exception));
        }
        else if (memory.Is(Memory.BsTypes.String, exception))
        {
            throw new InternalRaiseException(memory.GetStringValue(exception));
        }
        else
        {
            throw new InternalRaiseException("Unknown error, please panic");
        }
    }
}