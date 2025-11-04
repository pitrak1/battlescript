namespace Battlescript;

public class ContinueInstruction() : Instruction([])
{
    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        throw new InternalContinueException();
    }
}