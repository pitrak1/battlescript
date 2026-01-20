using System.Diagnostics;

namespace Battlescript;

public class RaiseInstruction : Instruction
{
    public Instruction? Value { get; set; }

    public RaiseInstruction(List<Token> tokens) : base(tokens)
    {
        Value = InstructionFactory.Create(tokens.GetRange(1, tokens.Count - 1));
    }
    
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var exception = Value?.Interpret(callStack, closure);
        // callStack.CurrentStack.AddFrame(this);

        if (BtlTypes.IsException(exception) && exception is ObjectVariable objectException)
        {
            var message = BtlTypes.GetErrorMessage(exception);
            var exceptionName = objectException.Class.Name;
            if (BtlTypes.TypeStrings.Contains(exceptionName))
            {
                throw new InternalRaiseException(BtlTypes.StringsToTypes[exceptionName], message);
            }
            else
            {
                throw new InternalRaiseException(exceptionName, BtlTypes.GetErrorMessage(exception));
            }
        }
        else
        {
            throw new InternalRaiseException("Unknown error, please panic");
        }
    }
}