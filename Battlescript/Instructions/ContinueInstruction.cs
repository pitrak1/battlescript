namespace Battlescript;

public class ContinueInstruction() : Instruction([])
{
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        throw new InternalContinueException();
    }
}