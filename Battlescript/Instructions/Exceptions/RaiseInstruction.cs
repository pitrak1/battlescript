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
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var exception = Value?.Interpret(callStack, closure);
        // callStack.CurrentStack.AddFrame(this);

        if (BsTypes.IsException(exception) && exception is ObjectVariable objectException)
        {
            var message = BsTypes.GetErrorMessage(exception);
            var exceptionName = objectException.Class.Name;
            if (BsTypes.TypeStrings.Contains(exceptionName))
            {
                throw new InternalRaiseException(BsTypes.StringsToTypes[exceptionName], message);
            }
            else
            {
                throw new InternalRaiseException(exceptionName, BsTypes.GetErrorMessage(exception));
            }
        }
        else
        {
            throw new InternalRaiseException("Unknown error, please panic");
        }
    }
}