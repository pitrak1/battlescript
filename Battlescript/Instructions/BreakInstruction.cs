namespace Battlescript;

public class BreakInstruction() : Instruction([])
{
    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        throw new InternalBreakException();
    }
}