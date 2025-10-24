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
        // memory.CurrentStack.AddFrame(this);

        if (memory.IsException(exception) && exception is ObjectVariable objectException)
        {
            var message = memory.GetErrorMessage(exception);
            var exceptionName = objectException.Class.Name;
            if (Memory.BsTypeStrings.Contains(exceptionName))
            {
                throw new InternalRaiseException(Memory.BsStringsToTypes[exceptionName], message);
            }
            else
            {
                throw new InternalRaiseException(exceptionName, memory.GetErrorMessage(exception));
            }
        }
        else
        {
            throw new InternalRaiseException("Unknown error, please panic");
        }
    }
}